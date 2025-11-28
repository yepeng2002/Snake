using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Client.WebApi
{
    public static class BasicAuthorizeExtensions
    {
        public static Dictionary<string, object> CreateDictionaryStringObject(this string postData)
        {
            // 转换为 Dic
            var dic = JsonHelper.DeserializeObject<Dictionary<string, object>>(postData);
            return dic;
        }

        /// <summary>
        /// 生成 验签
        /// PME Post
        /// </summary>
        /// <param name="postData">请求的参数</param>
        /// <param name="hzSecret">密钥</param>
        public static string CreatePostSign(this Dictionary<string, object> dic, string hzSecret)
        {
            if (!dic.ContainsKey("secret"))
            {
                // 添加 secret
                dic.Add("secret", hzSecret);
            }
            else
            {
                dic["secret"] = hzSecret;
            }
            // 排序
            var sorts = dic.OrderBy(p => p.Key);
            var sortDic = new Dictionary<string, object>();
            foreach (var item in sorts)
            {
                sortDic.Add(item.Key, item.Value);
            }
            var data = JsonHelper.SerializeObject(sortDic);
            // 全部转换为小写
            data = data.ToLower();
            // MD5 加密
            var result = EncryptHelper.MD5Encrypt(data);

            return result;
        }

        /// <summary>
        /// 生成 验签
        /// PME Get
        /// </summary>
        /// <param name="getUrl">请求的URL</param>
        /// <param name="hzSecret">密钥</param>
        public static string CreateGetSign(this string getUrl, string hzSecret)
        {
            // url 转换为小写
            getUrl = getUrl.ToLower();
            var result = EncryptHelper.MD5Encrypt(getUrl + "|" + hzSecret);

            return result;
        }
    }
}
