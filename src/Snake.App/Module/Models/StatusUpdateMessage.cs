using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.App.Module.Models
{
    /// <summary>
    /// 主界面状态栏消息更新事件类
    /// </summary>
    public class StatusUpdateMessage
    {
        public StatusUpdateMessage(string message)
        {
            Message = message;
        }
        public string Message { get; private set; }
    }
}
