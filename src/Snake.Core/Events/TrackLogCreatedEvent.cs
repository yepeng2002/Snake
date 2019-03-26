using Snake.Core.Models;
using Snake.Core.Mongo;
using System;
using System.Diagnostics;

namespace Snake.Core.Events
{
    [CollectionName("TrackLog")]
    public class TrackLogCreatedEvent : BaseEvent, ITrackLog
    {
        public static readonly string DefaultApplicationName = Process.GetCurrentProcess().MainModule.FileName;
        public static readonly string DefaultMachineName = Environment.MachineName;

        public TrackLogCreatedEvent()
        {
            GUID = Guid.NewGuid();
            CreateTime = DateTime.Now;
        }

        #region private set
        public Guid GUID { get; private set; }
        public DateTime CreateTime { get; private set; }
        #endregion

        public string RequestIP { get; set; }
        public int RequestPort { get; set; }
        public string RequestId { get; set; }
        public DateTime RequestTime { get; set; }
        public string RequestUrl { get; set; }
        public string ResponseApplication { get; set; }
        public string ResponseMachine { get; set; }
        public string ResponseIPv4 { get; set; }
        public string ResponseController { get; set; }
        public string ResponseAction { get; set; }
        public double? ExecutedTime { get; set; }
    }
}
