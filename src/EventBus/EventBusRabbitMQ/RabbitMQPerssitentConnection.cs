using RabbitMQ.Client;
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
    public class RabbitMQPerssitentConnection : IRabbitMQPersistentConnection
    {
        public bool IsConnected => throw new NotImplementedException();

        public IModel CreateModel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool TryConnect()
        {
            throw new NotImplementedException();
        }
    }
}
