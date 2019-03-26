using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Models
{
    public interface ITrackLog
    {
        string RequestIP { get; set; }

        int RequestPort { get; set; }

        string RequestId { get; set; }

        DateTime RequestTime { get; set; }

        string RequestUrl { get; set; }

        string ResponseApplication { get; set; }

        string ResponseIPv4 { get; set; }

        string ResponseMachine { get; set; }

        string ResponseController { get; set; }

        string ResponseAction { get; set; }
        /// <summary>
        /// 执行时间 ms
        /// </summary>
        double? ExecutedTime { get; set; }
    }
}
