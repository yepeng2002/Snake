using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Snake.Core.Events;
using Snake.Core.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Models
{
    /// <summary>
    /// 应用日志 CreatedEvent
    /// Mongodb集合：AppLog
    /// </summary>
    [CollectionName("AppLog")]
    public class AppLog : AppLogCreatedEvent, IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    /// <summary>
    /// 带分页的applog实体
    /// </summary>
    public class PageAppLog : AppLogCreatedEvent
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录条数
        /// </summary>
        public int PageSize { get; set; }
    }
}
