using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snake.Core.Util
{
    public class JsonHelper
    {
        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="value">序列化的对象</param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            string ret = string.Empty;
            try
            {
                ret = JsonConvert.SerializeObject(value);
            }
            catch (Exception exception)
            {
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// 将JSON字符串序列化成对象
        /// </summary>
        /// <param name="value">JSON字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
