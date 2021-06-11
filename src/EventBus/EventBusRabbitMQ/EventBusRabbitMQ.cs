using Autofac;
using EventBus;
using EventBus.Events;
using EventBus.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

/***************************************************************************************
*    Title: eShopOnContainers source code
*    Author: Microsoft
*    Date: 2021
*    Code version: 3.1.1
*    Availability: https://github.com/dotnet-architecture/eShopOnContainers
*
***************************************************************************************/

namespace EventBus.EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "hons_project_event_bus";
        const string SCOPE_NAME = "hons_project_event_bus";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _subscriptionsManager;
        private readonly ILifetimeScope _autofac;
        private readonly int _retryCount;

        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(
            IRabbitMQPersistentConnection persistentConnection, 
            ILogger<EventBusRabbitMQ> logger, 
            IEventBusSubscriptionsManager subscriptionsManager, 
            ILifetimeScope autofac, 
            string queueName,
            int retryCount = 5)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriptionsManager = subscriptionsManager ?? new InMemoryEventBusSubscriptionsManager();
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();
            _autofac = autofac;
            _retryCount = retryCount;
            _subscriptionsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            // if there is no connection
            if (!_persistentConnection.IsConnected)
            {
                // try to connect 
                _persistentConnection.TryConnect();
            }   

            using(var channel = _persistentConnection.CreateModel())
            {
                // bind the queue
                channel.QueueBind(
                    queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);

                // if there are no subscriptions 
                if (_subscriptionsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            // if there is no persistent connection then try connect
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            // Get the retry policy
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(
                    // retry a specifc number of times
                    _retryCount,
                    // calculate the sleep duration
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    // function to run on retry
                    (ex, time) =>
                    {
                        _logger.LogWarning(ex, $"RabbitMQ Client could not connect after {time.TotalSeconds:n1}s ({ex.Message})");
                    }
                );

            // get the event name
            var eventName = @event.GetType().Name;

            _logger.LogTrace($"Creating RabbitMQ channle to publish event: {@event.Id} ({eventName})");
            
            // create channel 
            using(var channel = _persistentConnection.CreateModel())
            {
                _logger.LogTrace($"Decalring RabbitMQ exchange to publish event: {@event.Id}");
                // declare exchange
                channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");
                // generate json body with event
                var body = JsonSerializer.SerializeToUtf8Bytes(
                    @event, 
                    @event.GetType(), 
                    new JsonSerializerOptions 
                    { 
                        WriteIndented = true
                    });
                // execute policy
                policy.Execute(() =>
                {
                    // create properties of channel
                    var properties = channel.CreateBasicProperties();
                    /**
                     * set the delivary mode
                     * 1 = non-persistent
                     * 2 = persistent
                     */
                    properties.DeliveryMode = 2;

                    _logger.LogTrace($"Publishing event to RabbitMQ: {@event.Id}");

                    // publish the event on the rabbitMQ channel
                    channel.BasicPublish(
                        exchange: BROKER_NAME,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            // get the event name
            var eventName = _subscriptionsManager.GetEventKey<T>();
            // do an internal subscription
            DoInternalSubscription(eventName);

            _logger.LogInformation($"Subscribing to event {eventName} with {typeof(TH).Name}");

            // add the subscription to the subscription manager
            _subscriptionsManager.AddSubscription<T, TH>();
            // start consume
            StartBasicConsume();
        }

        private void DoInternalSubscription(string eventName)
        {
            // check to see if there is a subscription for the event
            var hasKey = _subscriptionsManager.HasSubscriptionsForEvent(eventName);
            // if there is no subscription for this event with this handler
            if (!hasKey)
            {
                // if there is no connection then try connect
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                // using the rabbitmq channel
                using(var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(
                        queue: _queueName,
                        exchange: BROKER_NAME,
                        routingKey: eventName);
                }
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            // get the event name
            var eventName = _subscriptionsManager.GetEventKey<T>();

            _logger.LogInformation($"Unsubscribing from even {eventName}");

            // remove the subscription
            _subscriptionsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            // if the consumber channel exists
            if(_consumerChannel != null)
            {
                // dispose of the channel
                _consumerChannel.Dispose();
            }

            // clear the subscription manager
            _subscriptionsManager.Clear();
        }

        private void StartBasicConsume()
        {
            _logger.LogInformation("Starting RabbitMQ basic consume");

            // if there is a consumer channel
            if(_consumerChannel != null)
            {
                // create a basic consumer using the consumer channel
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                // when a delivary has been received by the consumer
                consumer.Received += Consumer_Received;
                // start a basic consume
                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can not call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            // get the event name
            var eventName = eventArgs.RoutingKey;
            // get the message body
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }
                // proccess the event
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"ERROR: \"{message}\"");
            }
        }

        private IModel CreateConsumerChannel()
        {
            // if there is no connection the try connect
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating a RabbitMQ consumer channel");

            // create a channel
            var channel = _persistentConnection.CreateModel();

            // delclare an exchange on the channel
            channel.ExchangeDeclare(
                exchange: BROKER_NAME,
                type: "direct");

            // declare the queue on the consumer channel
            channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // if there is a callback exceptioon the recreat the consumer channel 
            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ connsumer channel");

                // dispose of the consumer channel
                _consumerChannel.Dispose();
                // create a new consumer channel
                _consumerChannel = CreateConsumerChannel();
                // start a new basic consume
                StartBasicConsume();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace($"Proccessing RabbitMQ event: {eventName}");

            // if the subscritpion manager has the event 
            if (_subscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                // begin the autofac scope
                using (var scope = _autofac.BeginLifetimeScope(SCOPE_NAME))
                {
                    // get the handlers for this event
                    var subscriptions = _subscriptionsManager.GetHandlersForEvent(eventName);
                    // loop through each subsription
                    foreach(var sub in subscriptions)
                    {
                        // get the handler
                        var handler = scope.ResolveOptional(sub.HandlerType);
                        // if the handler is null then go to the next step in to for loop
                        if (handler == null) continue;
                        // get the event type
                        var eventType = _subscriptionsManager.GetEventTypeByName(eventName);
                        // get the integration event from the body
                        var integartionEvent = JsonSerializer.Deserialize(
                            message, 
                            eventType, 
                            new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        // get the concrete type
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        // invoke the handle method for the integration event
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integartionEvent });
                    }
                }
            }
            else
            {
                _logger.LogWarning($"No subscription for the RabbitMQ event: {eventName}");
            }
        }
    }
}
