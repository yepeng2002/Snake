using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Configurations
{
    public abstract class BusConfiguration
    {
        protected BusConfiguration()
        {
            ConnectObservers = null;
        }
        public abstract string RabbitMqHost { get; }
        public abstract ushort RabbitMqPort { get; }
        public abstract string RabbitMqVirtualHost { get; }
        public abstract string QueueName { get; }
        public abstract string RabbitMqUserName { get; }
        public abstract string RabbitMqPassword { get; }
        public abstract Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> Configuration { get; }
        public virtual Action<IBus> ConnectObservers { get; }

        public virtual IBus CreateBus()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(RabbitMqHost, RabbitMqPort, RabbitMqVirtualHost, hst =>
                {
                    hst.Username(RabbitMqUserName);
                    hst.Password(RabbitMqPassword);
                });

                Configuration?.Invoke(cfg, host);
            });

            ConnectObservers?.Invoke(bus);
            return bus;
        }
    }
}
