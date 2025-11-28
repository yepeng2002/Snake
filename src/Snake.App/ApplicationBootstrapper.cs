using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Snake.Core.Ioc;

namespace Snake.App
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
