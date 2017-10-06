using Snake.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MassTransit.RabbitMqTransport;
using MassTransit;
using GreenPipes;
using Snake.Core.Util;

namespace Snake.Api.ServiceBus
{
    public class TrackLogBusConfiguration : BusConfiguration
    {
        private TrackLogBusConfiguration() { }

        public override string RabbitMqHost { get; } = RabbitMQConfiguration.HostName;
        public override ushort RabbitMqPort { get; } = StringHelper.ToUshort(RabbitMQConfiguration.Port);
        public override string RabbitMqVirtualHost { get; } = RabbitMQConfiguration.VirtualHost;
        public override string QueueName { get; } = RabbitMQConfiguration.QueueName;
        public override string RabbitMqUserName { get; } = RabbitMQConfiguration.UserName;
        public override string RabbitMqPassword { get; } = RabbitMQConfiguration.Password;

        private static IBus _bus;

        public static IBus BusInstance => _bus ?? (_bus = new TrackLogBusConfiguration().CreateBus());

        public override Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> Configuration
        {
            get
            {
                return (cfg, host) =>
                {
                    cfg.UseRetry(Retry.Interval(RabbitMQConfiguration.UseRetryNum, TimeSpan.FromMinutes(1)));
                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                    });
                    host.GetSendAddress(QueueName);
                };
            }
        }        
    }
}