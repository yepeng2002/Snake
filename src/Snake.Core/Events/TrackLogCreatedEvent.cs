using Snake.Core.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Events
{
    [CollectionName("TrackLogCreatedEvent")]
    public class TrackLogCreatedEvent:BaseEvent
    {
        public string MachinName { get; set; }
    }
}
