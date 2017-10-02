using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Snake.ApiTrackService
{
    public class ApplicationBootstrapper
    {
        public static IWindsorContainer Container;

        public static IWindsorContainer RegisterContainer()
        {
            Container = new WindsorContainer();
            Container.Install(FromAssembly.InThisApplication());
            return Container;
        }
    }
}
