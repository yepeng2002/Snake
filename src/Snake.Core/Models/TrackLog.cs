using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace Snake.Core.Models
{
    public class TrackLog : ITrackLog
    {
        public static readonly string DefaultApplicationName = Process.GetCurrentProcess().MainModule.FileName;
        public static readonly string DefaultMachineName = Environment.MachineName;

        public TrackLog()
        {
            ResponseApplication = DefaultApplicationName;
            ResponseMachine = DefaultMachineName;
        }

        public string RequestIP { get; set; }
        public string ResponseApplication { get; set; }
        public string ResponseMachine { get; set; }
        public string ResponseIPv4 { get; set; }
        public int RequestPort { get; set; }
        public string RequestId { get; set; }
        public DateTime RequestTime { get; set; }
        public string RequestUrl { get; set; }
        public string ResponseController { get; set; }
        public string ResponseAction { get; set; }
        public double? ExecutedTime { get; set; }
    }
}