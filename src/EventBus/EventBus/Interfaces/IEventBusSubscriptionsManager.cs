using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Interfaces.InMemoryEventBusSubscriptionsManager;

/***************************************************************************************
*    Title: eShopOnContainers source code
*    Author: Microsoft
*    Date: 2021
*    Code version: 3.1.1
*    Availability: https://github.com/dotnet-architecture/eShopOnContainers
*
***************************************************************************************/

namespace EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        bool HasSubscriptionsForEvent<T>()
            where T : IntegrationEvent;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>()
            where T : IntegrationEvent;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();
    }
}
