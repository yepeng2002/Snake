using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using Snake.Api.ServiceBus;
using Snake.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snake.Api.ContainerInstallers
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IBus, IBusControl>()
                    .Instance(TrackLogBusConfiguration.BusInstance)
                    .LifestyleSingleton());

            container.Register(
                Classes.FromThisAssembly().BasedOn<BaseService>().WithServiceSelf().LifestyleTransient());
        }
    }
}