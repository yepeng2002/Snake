using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Snake.Client.WebApi
{
    public class HttpBaseProxy
    {
        protected virtual string GetContentType()
        {
            return "application/json";
        }

        protected HttpRequestResult Get(string url, Action<WebRequest> preHandler = null)
        {
            string result = null;
            int statusCode = -1;
            string message = null;
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(url);
                if (preHandler != null)
                {
                    preHandler(wReq);
                }
                System.Net.WebResponse wResp = wReq.GetResponse();
                using (System.IO.Stream respStream = wResp.GetResponseStream())
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                statusCode = 200;
            }
            catch (Exception ex)
            {
                statusCode = 0;
                message = ex.Message;
            }
            return new HttpRequestResult() { Code = statusCode, Data = result, Message = message };
        }

        protected HttpRequestResult Post(string url, Dictionary<string, object> postDataDic, Action<HttpWebRequest> preHandler = null)
        {
            string json = JsonHelper.SerializeObject(postDataDic);
            return Post(url, json, preHandler);
        }

        protected HttpRequestResult Post(string url, string postData, Action<HttpWebRequest> preHandler = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);
            return Post(url, data, preHandler);
        }

        protected HttpRequestResult Post(string url, byte[] data, Action<HttpWebRequest> preHandler = null)
        {
            string contentType = GetContentType();
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (preHandler != null)
            {
                preHandler(request);
            }
            request.Method = "post";
            request.ContentType = contentType;

            string result = null;
            int statusCode = -1;
            string message = null;
            try
            {
                //发送POST数据  
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse;
                using (Stream s = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    result = reader.ReadToEnd();
                    statusCode = (int)webResponse.StatusCode;
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse resp = (HttpWebResponse)wex.Response;
                message = wex.Message;
                if (resp != null)
                {
                    statusCode = (int)resp.StatusCode;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return new HttpRequestResult() { Code = statusCode, Data = result, Message = message };
        }

        protected void PostAsync(string url, Dictionary<string, object> postDataDic, Action<HttpRequestResult, object> callback, object requestState, Action<HttpWebRequest> preHandler = null)
        {
            string json = JsonHelper.SerializeObject(postDataDic);
            PostAsync(url, json, callback, requestState, preHandler);
        }

        protected void PostAsync(string url, string postData, Action<HttpRequestResult, object> callback, object requestState, Action<HttpWebRequest> preHandler = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);
            PostAsync(url, data, callback, requestState, preHandler);
        }

        protected void PostAsync(string url, byte[] data, Action<HttpRequestResult, object> callback, object requestState, Action<HttpWebRequest> preHandler = null)
        {
            string contentType = GetContentType();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (preHandler != null)
            {
                preHandler(request);
            }
            request.ContentType = contentType;
            request.KeepAlive = true;
            request.Method = WebRequestMethods.Http.Post;
            request.ContentLength = data.Length;
            HttpAsyncResult httpAsyncResult = new HttpAsyncResult(callback, data, requestState);
            httpAsyncResult.Request = request;
            try
            {
                request.BeginGetRequestStream(GetRequestStreamAsyncCallback, httpAsyncResult);
            }
            catch (Exception ex)
            {
                if (callback != null)
                {
                    HttpRequestResult httpRequestResult = new HttpRequestResult() { Code = -1, Data = null, Message = ex.Message };
                    callback(httpRequestResult, httpAsyncResult.RequestState);
                }
            }
        }

        void GetRequestStreamAsyncCallback(IAsyncResult ar)
        {
            HttpAsyncResult httpAsyncResult = ar.AsyncState as HttpAsyncResult;
            try
            {
                using (Stream requestStream = httpAsyncResult.Request.EndGetRequestStream(ar))
                {
                    requestStream.Write(httpAsyncResult.PostData, 0, httpAsyncResult.PostData.Length);
                    requestStream.Flush();
                }
                httpAsyncResult.Request.BeginGetResponse(new AsyncCallback(GetResponseCallback), httpAsyncResult);
            }
            catch (Exception ex)
            {
                if (httpAsyncResult.Callback != null)
                {
                    HttpRequestResult httpRequestResult = new HttpRequestResult() { Code = -1, Data = null, Message = ex.Message };
                    httpAsyncResult.Callback(httpRequestResult, httpAsyncResult.RequestState);
                }
            }
        }

        void GetResponseCallback(IAsyncResult ar)
        {
            HttpAsyncResult httpAsyncResult = ar.AsyncState as HttpAsyncResult;
            string result = null;
            int statusCode = -1;
            string message = null;
            try
            {
                HttpWebResponse webResponse = httpAsyncResult.Request.EndGetResponse(ar) as HttpWebResponse;
                using (Stream s = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    result = reader.ReadToEnd();
                    statusCode = (int)webResponse.StatusCode;
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse resp = (HttpWebResponse)wex.Response;
                message = wex.Message;
                if (resp != null)
                {
                    statusCode = (int)resp.StatusCode;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            if (httpAsyncResult.Callback != null)
            {
                HttpRequestResult httpRequestResult = new HttpRequestResult() { Code = statusCode, Data = result, Message = message };
                httpAsyncResult.Callback(httpRequestResult, httpAsyncResult.RequestState);
            }
        }

        string GetServerInfo()
        {
            string serverIp = System.Web.HttpContext.Current.Request.ServerVariables["Local_Addr"].ToString();
            string serverName = System.Web.HttpContext.Current.Request.ServerVariables["Server_Name"];
            string serverPort = System.Web.HttpContext.Current.Request.ServerVariables["Server_Port"];
            string append = string.Format("name:{0} ip:{1} port:{2}", serverName, serverIp, serverPort);
            return append;
        }

        class HttpAsyncResult : IAsyncResult
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
    }

}
