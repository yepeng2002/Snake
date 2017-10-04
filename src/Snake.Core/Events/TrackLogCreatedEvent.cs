using Snake.Core.Mongo;
using System;
using System.Diagnostics;

namespace Snake.Core.Events
{
    [CollectionName("TrackLog")]
    public class TrackLogCreatedEvent:BaseEvent
    {
        public static readonly string DefaultApplicationName = Process.GetCurrentProcess().MainModule.FileName;
        public static readonly string DefaultMachineName = Environment.MachineName;

        public TrackLogCreatedEvent()
        {
            GUID = Guid.NewGuid();
            CreateTime = DateTime.Now;
            Machine = DefaultMachineName;
            FromApplication = DefaultApplicationName;
        }

        public Guid GUID { get; private set; }
        public DateTime CreateTime { get; private set; }
        public string Machine { get; private set; }
        public string FromApplication { get; private set; }

        public DateTime? UpdateTime { get; set; }
        public bool Ack { get; set; }
        public string Exception { get; set; }
        public string Body { get; set; }
    }
}
