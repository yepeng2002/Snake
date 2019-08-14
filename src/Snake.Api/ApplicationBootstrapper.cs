using Castle.Windsor;
using Castle.Windsor.Installer;
using Snake.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snake.Api
{
    public class ApplicationBootstrapper
    {
        public static IWindsorContainer Container;

        public static IWindsorContainer RegisterContainer()
        {
            Container = IocContainer.GetContainer();
            Container.Install(FromAssembly.InThisApplication());
            return Container;
        }
    }
}