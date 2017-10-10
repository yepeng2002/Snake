using Snake.Client.WebApi;
using Snake.Core.Models;
using Snake.Core.Util;
using System;
using System.Diagnostics;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Snake.Client.Filters
{
    public class TrackLogActionFilterAttribute : ActionFilterAttribute
    {
        private const string Key = "__action_duration__";
        private const string RequestTimeKey = "__action_request__";
        private const string RequestKey = "__action_requestid__";

        /// <summary>
        /// 启用计时器
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requestTime = DateTime.Now;
            actionContext.Request.Properties[RequestTimeKey] = DateHelper.DateTimeToUnixStamp(requestTime);
            string requestId = Guid.NewGuid().ToString();
            actionContext.Request.Properties[RequestKey] = requestId;
            var stopWatch = new Stopwatch();
            actionContext.Request.Properties[Key] = stopWatch;
            stopWatch.Start();

            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                SnakeWebApiHttpProxy snakeWebApiHttpProxy = new SnakeWebApiHttpProxy();
                snakeWebApiHttpProxy.PublishTrackLog<string>(new TrackLog()
                {
                    AbsolutePath = actionContext.Request.RequestUri.AbsolutePath,
                    Port = actionContext.Request.RequestUri.Port,
                    RequestId = requestId,
                    RequestTime = requestTime,
                    Url = actionContext.RequestContext.Url.Request.RequestUri.OriginalString,
                    ControllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                    ActionName = actionContext.ActionDescriptor.ActionName
                });
            }));
        }

        /// <summary>
        /// 记录监控接口执行日志
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            DateTime requestTime = DateHelper.UnixStampToDateTme(Convert.ToInt64(actionExecutedContext.Request.Properties[RequestTimeKey]));
            string requestId = actionExecutedContext.Request.Properties[RequestKey].ToString();
            var stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;
            if (stopWatch != null)
            {
                stopWatch.Stop();
                var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string log = string.Format("Thread:{0}  {1}.{2}   Take {3} ms.", Thread.CurrentThread.ManagedThreadId, controllerName, actionName, stopWatch.Elapsed.TotalMilliseconds.ToString("0."));
                Trace.WriteLine(log);

                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    SnakeWebApiHttpProxy snakeWebApiHttpProxy = new SnakeWebApiHttpProxy();
                    snakeWebApiHttpProxy.PublishTrackLog<string>(new TrackLog()
                    {
                        AbsolutePath = actionExecutedContext.Request.RequestUri.AbsolutePath,
                        Port = actionExecutedContext.Request.RequestUri.Port,
                        RequestId = requestId,
                        RequestTime = requestTime,
                        Url = actionExecutedContext.Request.RequestUri.OriginalString,
                        ControllerName = controllerName,
                        ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                        ExecutedTime = stopWatch.Elapsed.TotalMilliseconds
                    });
                }));
            }
        }        
    }
}
