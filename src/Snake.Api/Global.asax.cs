using Castle.MicroKernel.Registration;
using log4net;
using MassTransit;
using Snake.Api.ServiceBus;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Snake.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IBusControl _bus;

        private void RegisterBus()
        {
            ApplicationBootstrapper.Container.Register(
                Component.For<IBus, IBusControl>()
                    .Instance(TrackLogBusConfiguration.BusInstance)
                    .LifestyleSingleton());
        }

        protected void Application_Start()
        {
            //************************** Default *************************
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //*************************************************************

            //************************** bus ******************************
            var container = ApplicationBootstrapper.RegisterContainer();
            RegisterBus();
            _bus = container.Resolve<IBusControl>();
            _bus.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var _log = LogManager.GetLogger(typeof(WebApiApplication));
            _log.Error("Unhandled exception", Server.GetLastError().GetBaseException());
        }

        protected void Application_End()
        {
            _bus.Stop();
        }
    }
}
