using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

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
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQPersistentConnection> _logger;
        private readonly int _retryCount;
        IConnection _connection;
        bool _disposed;

        object sync_root = new object();
        
        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQPersistentConnection> logger, int retryCount = 5)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _retryCount = retryCount;
        }

        public bool IsConnected
        {
            get
            {
                // true if there is a connection and the connection is opened and the connection has not been disposed
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateModel()
        {
            // if there is no connection
            if (!IsConnected)
            {
                throw new InvalidOperationException("There are no RabbitMQ connections available to peform this action");
            }
            // create the model using the connection
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            // if the connection has already been disposed
            if (_disposed) return;

            _disposed = true;

            try
            {
                // dispose of the connection
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                // log IO errors
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            // lock so that other threads dont run this critical code at the same time
            lock (sync_root)
            {
                // Get the retry policy
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
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

                // execute the policy
                policy.Execute(() =>
                {
                    // create the connection
                    _connection = _connectionFactory.CreateConnection();
                });

                // if the connection to RabbitMQ was made 
                if (IsConnected)
                {
                    // on connection shutdown then try reconnect
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    // on call back exception then try reconnect
                    _connection.CallbackException += OnCallBackException;
                    // on connection blocked then try reconnect
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation($"RabbitMQ Client has made a persistent connection to '{_connection.Endpoint.HostName}'");

                    // connection has been made
                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connection could not be created and opened");

                    // connection could not be made
                    return false;
                }
            }
        }
        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is blocked. Reconnecting...");

            TryConnect();
        }

        private void OnCallBackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ connection thew exception. Reconnecting...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ connection is shutdown. Reconnecting...");

            TryConnect();
        }
    }
}
