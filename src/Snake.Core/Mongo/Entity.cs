using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Snake.Core.Mongo
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
