using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Models
{
    public interface ITrackLog
    {
        string FromApplication { get; set; }
        string FromMachine { get; set; }
        string RequestId { get; set; }
        DateTime RequestTime { get; set; }
        string Url { get; set; }
        string ControllerName { get; set; }
        string ActionName { get; set; }
        /// <summary>
        /// 执行时间 ms
        /// </summary>
        double? ExecutedTime { get; set; }
    }
}
