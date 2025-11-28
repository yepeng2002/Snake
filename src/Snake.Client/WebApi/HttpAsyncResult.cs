using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Snake.Client.WebApi
{
    public class HttpAsyncResult : IAsyncResult
    {

        public byte[] PostData { get; set; }

        public Action<HttpRequestResult, object> Callback { get; set; }

        public object RequestState { get; set; }

        public HttpWebRequest Request { get; set; }

        #region IAsyncResult Members
        public object AsyncState { get; private set; }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException("Do not use this AsyncWaitHandle."); }
        }

        public bool CompletedSynchronously { get; set; }

        public bool IsCompleted { get; set; }

        #endregion

        public HttpAsyncResult(Action<HttpRequestResult, object> callback, byte[] postData, object requestState)
        {
            Callback = callback;
            PostData = postData;
            RequestState = requestState;
        }
    }

    /// <summary>
    /// Summary description for RequestState
    /// </summary>
    public class RequestState
    {
        const int BUFFER_SIZE = 1024;
        public StringBuilder RequestData;
        public byte[] BufferRead;
        public HttpWebRequest Request;
        public Stream ResponseStream;
        // 创建适当编码类型的解码器
        public Decoder StreamDecode = Encoding.UTF8.GetDecoder();

        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            RequestData = new StringBuilder("");
            Request = null;
            ResponseStream = null;
        }
    }
}
