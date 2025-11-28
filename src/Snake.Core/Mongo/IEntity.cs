using MongoDB.Bson.Serialization.Attributes;

namespace Snake.Core.Mongo
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}
