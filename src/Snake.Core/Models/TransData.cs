using Snake.Core.Enums;

namespace Snake.Core.Models
{
    /// <summary>
    /// 响应结果实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TransData<T>
    {
        /// <summary>
        /// 请求响应结果编码
        /// </summary>
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public TransData()
        {
            Code = (int)ServiceResultCode.Succeeded;
        }
    }
}