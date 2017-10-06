using Snake.Client.WebApi;
using Snake.Core.Enums;
using Snake.Core.Models;
using System.Diagnostics;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Snake.Client.Filters
{
    public class TrackLogActionFilterAttribute : ActionFilterAttribute
    {
        private const string Key = "__action_duration__";

        /// <summary>
        /// 启用计时器
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var stopWatch = new Stopwatch();
            actionContext.Request.Properties[Key] = stopWatch;
            stopWatch.Start();

            SnakeWebApiHttpProxy snakeWebApiHttpProxy = new SnakeWebApiHttpProxy();
            snakeWebApiHttpProxy.PublishTrackLog<string>(new TrackLog());
        }

        /// <summary>
        /// 记录监控接口执行日志
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;
            if (stopWatch != null)
            {
                stopWatch.Stop();
                var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string log = string.Format("Thread:{0}  {1}.{2}   Take {3} ms.", Thread.CurrentThread.ManagedThreadId, controllerName, actionName, stopWatch.Elapsed.TotalMilliseconds.ToString("0."));
                Trace.WriteLine(log);
                SnakeWebApiHttpProxy snakeWebApiHttpProxy = new SnakeWebApiHttpProxy();
                snakeWebApiHttpProxy.PublishTrackLog<string>(new TrackLog());
            }
        }        
    }
}
