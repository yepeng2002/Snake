using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Snake.Core.Configurations;
using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.ApiTrackService
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
                    cfg.UseRetry(Retry.Interval(RabbitMQConfiguration.UseRetryNum, TimeSpan.FromSeconds(10)));  //重试配置
                    //cfg.UseRateLimit(1000, TimeSpan.FromSeconds(1));  //限速器
                    cfg.UseConcurrencyLimit(RabbitMQConfiguration.ConsumerNum); //消费者线程个数
                    cfg.ReceiveEndpoint(host, QueueName, e =>
                    {
                        //e.AutoDelete = false;
                        //e.Durable = true;
                        //e.ExchangeType = "fanout";
                        //e.UseConcurrencyLimit(RabbitMQConfiguration.ConsumerNum);
                        e.UseMessageScope();
                        //1. register consumers by manually
                        //e.Consumer<TrackLogCreatedEventConsumer>();
                        //e.Consumer<TrackLogUpdatedEventConsumer>();

                        //2. register comsumers by container
                        e.LoadFrom(ApplicationBootstrapper.Container);
                    });
                };
            }
        }

        public override Action<IBus> ConnectObservers
        {
            get
            {
                return bus =>
                {
                    //bus.ConnectReceiveObserver(new ReceiveObserver());
                    //bus.ConnectConsumeMessageObserver(new ConsumeObserver<UserCreatedEvent>());
                };
            }
        }

    }
}
