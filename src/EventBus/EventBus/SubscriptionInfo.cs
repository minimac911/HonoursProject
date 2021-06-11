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
        public class SubscriptionInfo
        {
            public bool IsDynamic { get; }
            public Type HandlerType { get; }

            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            public static SubscriptionInfo Dynamic(Type handlerType)
            {
                return new SubscriptionInfo(true, handlerType);
            }

            public static SubscriptionInfo Typed(Type handlerType)
            {
                return new SubscriptionInfo(false, handlerType);
            }
        }
    }
}
