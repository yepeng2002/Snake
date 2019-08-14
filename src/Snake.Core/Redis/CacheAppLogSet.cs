using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Redis
{
    /// <summary>
    /// AppLog缓存集合名
    /// </summary>
    public static class CacheAppLogSet
    {
        /// <summary>
        /// Application缓存集合id
        /// </summary>
        public static string Application
        {
            get { return "Application"; }
        }

        /// <summary>
        /// Tags缓存集合id
        /// </summary>
        public static string Tags
        {
            get { return "Tags"; }
        }
    }
}
