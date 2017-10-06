using Snake.Client.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Snake.DemoApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            
            config.Filters.Add(new TrackLogActionFilterAttribute());  //api执行事件跟踪日志

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
