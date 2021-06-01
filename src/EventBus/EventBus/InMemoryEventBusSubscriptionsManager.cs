using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/***************************************************************************************
*    Title: eShopOnContainers source code
*    Author: Microsoft
*    Date: 2021
*    Code version: 3.1.1
*    Availability: https://github.com/dotnet-architecture/eShopOnContainers
*
***************************************************************************************/

namespace EventBus.Interfaces
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        bool IEventBusSubscriptionsManager.IsEmpty => throw new NotImplementedException();

        public event EventHandler<string> OnEventRemoeved;
        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        public bool IsEmpty() => !_handlers.Keys.Any();

        public void Clear()
        {
            _handlers.Clear();
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            // get the event name
            var eventName = GetEventKey<T>();

            // add the subscription to the event
            DoAddSubscription(typeof(TH), eventName);

            // if the event type is new then add it to the event type list
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        private void DoAddSubscription(Type handlerType, string eventName)
        {
            // Check if there is already a subscription for the event
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            // if the handler type already exists for the event 
            if(_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"The handler type {handlerType.Name} has already been registered for the event: {eventName}", nameof(handlerType));
            }
            
            // add a subscription to the event
            _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            // get the handler that needs to be remvoed
            var handlerToBeRemoved = FindSubscriptionToRemove<T, TH>();
            // get the event name for the event
            var eventName = GetEventKey<T>();
            // Remove the handler for the event
            DoRemoveHandler(eventName, handlerToBeRemoved);
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo subToRemove)
        {
            // if the sub exists
            if(subToRemove != null)
            {
                // remvoe the subscription from the event
                _handlers[eventName].Remove(subToRemove);
                // if there are no more handlers for the event 
                if (!_handlers[eventName].Any())
                {
                    // remove the event 
                    _handlers.Remove(eventName);
                    // get the event type using the event name
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                    //if the event type exists
                    if(eventType != null)
                    {
                        // remove the event type
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoeved;
            handler?.Invoke(this, eventName);
        }

        private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
        {
            // get the event name
            var eventName = GetEventKey<T>();
            // find thesubscription using the event name and the handler type
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            // if there are no subscriptions with that event name
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }
            
            // get the subscription using the event name and the handler type
            var subscriptionFound = _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);

            return subscriptionFound;
        }


        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            // get the event key
            var eventName = GetEventKey<T>();

            return HasSubscriptionsForEvent(eventName);
        }

        public bool HasSubscriptionsForEvent(string eventName)
        {
            // check if the handlers contain the event name
            return _handlers.ContainsKey(eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            // get the event key
            var eventKey = GetEventKey<T>();
            // get the handler for the event key
            return GetHandlersForEvent(eventKey);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
        {
            // get the handler with the given event name
            return _handlers[eventName];
        }

        public Type GetEventTypeByName(string eventName)
        {
            // get the event type using the name of the event
            return _eventTypes.SingleOrDefault(t => t.Name == eventName);
        }

        public string GetEventKey<T>()
        {
            // get the event name
            return typeof(T).Name;
        }
    }
}
