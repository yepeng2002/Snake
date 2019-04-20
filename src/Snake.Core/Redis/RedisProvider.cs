using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Redis
{
    public class RedisProvider : ICacheProvider
    {

        #region 字段
        private Lazy<long> _defaultExpireTime;
        private const double ExpireTime = 60D;
        private string _keySuffix;
        private IRedisClient _redisClient;
        #endregion

        #region 构造函数

        public RedisProvider(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 添加K/V值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(string key, object value)
        {
            this.Add(key, value, TimeSpan.FromSeconds(ExpireTime));
        }

        /// <summary>
        /// 异步添加K/V值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void AddAsync(string key, object value)
        {
            this.AddTaskAsync(key, value, TimeSpan.FromMinutes(ExpireTime));
        }

        /// <summary>
        /// 添加k/v值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="defaultExpire">默认配置失效时间</param>
        public void Add(string key, object value, bool defaultExpire)
        {
            this.Add(key, value, TimeSpan.FromMinutes(defaultExpire ? DefaultExpireTime : ExpireTime));
        }

        /// <summary>
        /// 异步添加K/V值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="defaultExpire">默认配置失效时间</param>
        public void AddAsync(string key, object value, bool defaultExpire)
        {
            this.AddTaskAsync(key, value, TimeSpan.FromMinutes(defaultExpire ? DefaultExpireTime : ExpireTime));
        }

        /// <summary>
        /// 添加k/v值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="numOfMinutes">默认配置失效时间</param>
        public void Add(string key, object value, long numOfMinutes)
        {
            this.Add(key, value, TimeSpan.FromMinutes(numOfMinutes));
        }


        /// <summary>
        /// 异步添加K/V值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="numOfMinutes">默认配置失效时间</param>
        public void AddAsync(string key, object value, long numOfMinutes)
        {
            this.AddTaskAsync(key, value, TimeSpan.FromMinutes(numOfMinutes));
        }


        /// <summary>
        /// 添加k/v值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeSpan">配置时间间隔</param>
        public void Add(string key, object value, TimeSpan timeSpan)
        {
            _redisClient.Set(GetKeySuffix(key), value, timeSpan);
        }

        /// <summary>
        /// 异步添加K/V值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeSpan">配置时间间隔</param>
        public void AddAsync(string key, object value, TimeSpan timeSpan)
        {
            this.AddTaskAsync(key, value, timeSpan);
        }

        /// <summary>
        /// 添加k/v值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="numOfMinutes">配置时间间隔</param>
        /// <param name="flag">标识是否永不过期（NETCache本地缓存有效）</param>
        public void Add(string key, object value, long numOfMinutes, bool flag)
        {
            this.Add(key, value, TimeSpan.FromMinutes(numOfMinutes));
        }

        /// <summary>
        /// 异步添加K/V值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="numOfMinutes">配置时间间隔</param>
        /// <param name="flag">标识是否永不过期（NETCache本地缓存有效）</param>
        public void AddAsync(string key, object value, long numOfMinutes, bool flag)
        {
            this.AddTaskAsync(key, value, TimeSpan.FromMinutes(numOfMinutes));
        }

        /// <summary>
        /// 根据KEY键集合获取返回对象集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="keys">KEY值集合</param>
        /// <returns>需要返回的对象集合</returns>
        public IDictionary<string, T> Get<T>(IEnumerable<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                this.Add(key, this.Get<T>(key));
            }
            return result;
        }

        /// <summary>
        /// 根据KEY键集合异步获取返回对象集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="keys">KEY值集合</param>
        /// <returns>需要返回的对象集合</returns>
        public async Task<IDictionary<string, T>> GetAsync<T>(IEnumerable<string> keys)
        {
            var result = new Task<Dictionary<string, T>>(() => new Dictionary<string, T>());
            foreach (var key in keys)
            {
                this.Add(key, await Task.Run(() => _redisClient.Get<T>(GetKeySuffix(key))));
            }
            return result.Result;
        }

        /// <summary>
        /// 根据KEY键获取返回对象
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <returns>需要返回的对象</returns>
        public object Get(string key)
        {
            var o = this.Get<object>(key);
            return o;
        }

        /// <summary>
        /// 根据KEY异步获取返回对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(string key)
        {
            var result = await this.GetTaskAsync<object>(key);
            return result;
        }

        /// <summary>
        /// 根据KEY键获取返回指定的类型对象
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">KEY值</param>
        /// <returns>需要返回的对象</returns>
        public T Get<T>(string key)
        {
            var result = default(T);
            result = _redisClient.Get<T>(GetKeySuffix(key));
            return result;
        }



        /// <summary>
        /// 根据KEY异步获取指定的类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var result = await Task.Run(() => _redisClient.Get<T>(GetKeySuffix(key)));
            return result;

        }

        /// <summary>
        /// 根据KEY键获取转化成指定的对象，指示获取转化是否成功的返回值
        /// </summary>
        /// <param name="key">KEY键</param>
        /// <param name="obj">需要转化返回的对象</param>
        /// <returns>是否成功</returns>
        public bool GetCacheTryParse(string key, out object obj)
        {
            obj = null;
            var o = this.Get<object>(key);
            return o != null;
        }

        /// <summary>
        /// 根据KEY键删除缓存
        /// </summary>
        /// <param name="key">KEY键</param>
        public void Remove(string key)
        {
            _redisClient.Remove(GetKeySuffix(key));
        }

        /// <summary>
        /// 根据KEY键异步删除缓存
        /// </summary>
        /// <param name="key">KEY键</param>
        public void RemoveAsync(string key)
        {
            this.RemoveTaskAsync(key);
        }

        public long DefaultExpireTime
        {
            get
            {
                return _defaultExpireTime.Value;
            }
            set
            {
                _defaultExpireTime = new Lazy<long>(() => value);
            }

        }

        public string KeySuffix
        {
            get
            {
                return _keySuffix;
            }
            set
            {
                _keySuffix = value;
            }
        }

        /// <summary>
        /// 对Set类型进行插入item操作
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="item"></param>
        public void AddItemToSet(string setId, string item)
        {
            _redisClient.AddItemToSet(setId, item);
        }

        public async void AddItemToSetAsync(string setId, string item)
        {
            await Task.Run(() => this.AddItemToSet(setId, item));
        }
        public void AddRangeToSet(string setId, IList<string> items)
        {
            _redisClient.AddRangeToSet(setId, new List<string>(items));
        }

        public HashSet<string> GetAllItemsFromSet(string setId)
        {
            return _redisClient.GetAllItemsFromSet(setId);
        }
        
        #endregion

        #region 私有方法


        private async Task<T> GetTaskAsync<T>(string key)
        {
            return await Task.Run(() => this.Get<T>(key));
        }

        private async void AddTaskAsync(string key, object value, TimeSpan timeSpan)
        {
            await Task.Run(() => this.Add(key, value, timeSpan));
        }

        private async void RemoveTaskAsync(string key)
        {
            await Task.Run(() => this.Remove(key));
        }

        private string GetKeySuffix(string key)
        {
            return string.IsNullOrEmpty(KeySuffix) ? key : string.Format("_{0}_{1}", KeySuffix, key);
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            _redisClient.Dispose();
        }
        #endregion

    }
}
