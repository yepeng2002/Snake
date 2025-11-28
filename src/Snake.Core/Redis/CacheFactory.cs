using ServiceStack.Common.Net30;
using ServiceStack.Redis;
using Snake.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Redis
{
    public class CacheFactory
    {
        //private const int REDIS_DB1 = 1;
        private static CacheFactory _cacheFactory;
        private static System.Collections.Concurrent.ConcurrentDictionary<string, PooledRedisClientManager > _pcms = new System.Collections.Concurrent.ConcurrentDictionary<string, PooledRedisClientManager>();
        private static ConcurrentQueue<RedisProvider> RedisList = new ConcurrentQueue<RedisProvider>();
        private static object _lock = new object();

        public static CacheFactory Instance
        {
            get
            {
                if (_cacheFactory == null)
                {
                    lock (_lock)
                    {
                        if (_cacheFactory == null)
                        {
                            _cacheFactory = new CacheFactory();
                        }
                    }
                }

                return _cacheFactory;
            }
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        private IRedisClient GetRedisClient(string sectionName = "RedisConfig")
        {
            PooledRedisClientManager pcm;
            if (!_pcms.TryGetValue(sectionName, out pcm))
            {
                var redisConfigInfo = RedisConfigInfo.GetConfig(sectionName);
                string[] writeServerList = SplitString(redisConfigInfo.WriteServerList, ",");
                string[] readServerList = SplitString(redisConfigInfo.ReadServerList, ",");
                pcm = new PooledRedisClientManager(readServerList, writeServerList,
                                 new RedisClientManagerConfig
                                 {
                                     MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                                     MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                                     AutoStart = redisConfigInfo.AutoStart,
                                     DefaultDb = redisConfigInfo.DefaultDb,
                                 });
                pcm.ConnectTimeout = 2 * 60 * 1000;
                _pcms[sectionName] = pcm;
            }

            if (pcm == null)
            {
                throw new InvalidOperationException("Redis尚未初始化或失败，无法打开客户端！");
            }


            IRedisClient client = null;
            try
            {
                client = pcm.GetClient();//获取连接;
            }
            catch (TimeoutException)
            {
                client = pcm.GetClient();
            }
            catch (Exception ex)
            {
                return null;
            }
            return client;//获取连接;     
        }

        public ICacheProvider GetClient(CacheTargetType cacheTargetType = CacheTargetType.Redis, string sectionName = "RedisConfig")
        {
            switch (cacheTargetType)
            {
                case CacheTargetType.Redis:
                    {
                        var redisClient = GetRedisClient(sectionName);
                        if (redisClient == null)
                            return null;
                        return new RedisProvider(redisClient);
                    }
                default:
                    {
                        var redisClient = GetRedisClient(sectionName);
                        if (redisClient == null)
                            return null;
                        return new RedisProvider(redisClient);
                    }
            }
        }
    }
}
