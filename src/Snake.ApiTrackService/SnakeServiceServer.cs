using Castle.MicroKernel.Registration;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Snake.ApiTrackService
{
    public class SnakeServiceServer : ServiceControl
    {

        private readonly IBusControl _bus;

        public SnakeServiceServer()
        {
            var container = ApplicationBootstrapper.RegisterContainer();
            RegisterBus();
            _bus = container.Resolve<IBusControl>();
        }

        private void RegisterBus()
        {
            ApplicationBootstrapper.Container.Register(
                Component.For<IBus, IBusControl>()
                    .Instance(TrackLogBusConfiguration.BusInstance)
                    .LifestyleSingleton());
        }
        
        public bool Start(HostControl hostControl)
        {
            _bus.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus.Stop();
            return true;
        }
    }
}
