using MongoDB.Bson.Serialization.Attributes;
using Snake.Core.IMessage;
using MongoDB.Bson;
using Snake.Core.Mongo;
using System.ComponentModel;

namespace Snake.Core.Events
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class BaseEvent: NotifyEntity, IEvent
    {
    }
}
