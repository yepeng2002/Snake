using MongoDB.Bson.Serialization.Attributes;
using Snake.Core.IMessage;
using MongoDB.Bson;
using Snake.Core.Mongo;

namespace Snake.Core.Events
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class BaseEvent:IEvent,IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
