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
            FromApplication = DefaultApplicationName;
            FromMachine = DefaultMachineName;
        }

        public string FromApplication { get; set; }
        public string FromMachine { get; set; }
        public DateTime RequestTime { get; set; }
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public double? ExecutedTime { get; set; }
    }
}