using Snake.Client.WebApi;
using Snake.Core.Enums;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake.Client
{
    public static class LogProxy
    {
        private static void Log(string message, string application, int level, LogCategory logCategory, IList<string> tags)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                SnakeWebApiHttpProxy snakeWebApiHttpProxy = new SnakeWebApiHttpProxy();
                snakeWebApiHttpProxy.PublishAppLog<string>(new AppLogCreatedEvent()
                {
                    Guid = Guid.NewGuid().ToString(),
                    CTime = DateTime.Now,
                    IPv4 = UrlHelper.GetIPv4(),
                    Application = application,
                    AppPath = Process.GetCurrentProcess().MainModule.FileName,
                    Machine = Environment.MachineName,
                    LogCategory = logCategory.ToString(),
                    Message = message,
                    Level = level,
                    Tags = tags
                });
            }));
        }

        public static void Fatal(string message, string application = "", int level = 1, IList<string> tags = null)
        {
            Log(message, application, level, LogCategory.Fatal, tags);
        }

        public static void Error(string message, string application = "", int level = 3, IList<string> tags = null)
        {
            Log(message, application, level, LogCategory.Error, tags);
        }
        
        public static void Info(string message, string application = "", int level = 5, IList<string> tags = null)
        {
            Log(message, application, level, LogCategory.Info, tags);
        }

        public static void Debug(string message, string application = "", int level = 5, IList<string> tags = null)
        {
            Log(message, application, level, LogCategory.Debug, tags);
        }
    }
}
