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
        public override string ExchangeName { get; } = RabbitMQConfiguration.ExchangeName;
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
                    //Retry: 重试
                    cfg.UseRetry(Retry.Interval(RabbitMQConfiguration.UseRetryNum, TimeSpan.FromMinutes(1)));
                    //// RateLimit : 限流
                    ////cfg.UseRateLimit(1000, TimeSpan.FromMinutes(1)); // 1分钟以内最多1000次调用访问
                    //// CircuitBreaker : 熔断
                    //cfg.UseCircuitBreaker(cb =>
                    //{
                    //    cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                    //    cb.TripThreshold = 15; // 当失败的比例至少达到15%才会启动熔断
                    //    cb.ActiveThreshold = 10; // 当失败次数至少达到10次才会启动熔断
                    //    cb.ResetInterval = TimeSpan.FromMinutes(5);
                    //}); // 当在1分钟内消费失败率达到15%或调用了10次还是失败时，暂停5分钟的服务访问
                    host.GetSendAddress(ExchangeName);
                };
            }
        }        
    }
}