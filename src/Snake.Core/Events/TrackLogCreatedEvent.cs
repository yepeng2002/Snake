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

        public string FromApplication { get; set; }
        public string FromMachine { get; set; }
        public string RequestId { get; set; }
        public DateTime RequestTime { get; set; }
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public double? ExecutedTime { get; set; }
    }
}
