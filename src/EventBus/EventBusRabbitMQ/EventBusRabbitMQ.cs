using EventBus.Events;
using EventBus.Interfaces;
using Microsoft.Extensions.Logging;
using System;

/***************************************************************************************
*    Title: eShopOnContainers source code
*    Author: Microsoft
*    Date: 2021
*    Code version: 3.1.1
*    Availability: https://github.com/dotnet-architecture/eShopOnContainers
*
***************************************************************************************/

namespace EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "hons_project_event_bus";
        const string SCOPE_NAMNE = "hons_project_event_bus";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBus
        private readonly
        private readonly

        void IEventBus.Publish(IntegrationEvent @event)
        {
            throw new NotImplementedException();
        }

        void IEventBus.Subscribe<T, TH>()
        {
            throw new NotImplementedException();
        }

        void IEventBus.Unsubscribe<T, TH>()
        {
            throw new NotImplementedException();
        }
    }
}
