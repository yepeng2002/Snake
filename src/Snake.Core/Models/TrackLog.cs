using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Snake.Core.Events;
using Snake.Core.Mongo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace Snake.Core.Models
{
    /// <summary>
    /// mongodb: api日志数据集
    /// </summary>
    [CollectionName("TrackLog")]
    public class TrackLog : TrackLogCreatedEvent, IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}