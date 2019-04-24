using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Client.WebApi
{
    public class SnakeWebApiHttpProxy : HttpBaseProxy
    {
        #region 1. 自定义常量

        const string SNAKE_API_KEY = "SnakeApi";
        const string SNAKE_API_SECRET = "SnakeApiSecret";
        const string SNAKE_API = "SnakeServerApi";
        const string SNAKE_WEBAPI_TRACKLOG_CONTROLLER = "api/TrackLog";

        #endregion

        #region 2. HttpClient
        private string _baseUrl, _emFlaskApiKey, _emFlaskApiSercret;
        protected string _serviceLinkFlag; //Dev, Test, Prd

        public SnakeWebApiHttpProxy()
        {
            //用于发送get请求到EMFLASKAPI
            _emFlaskApiKey = ConfigurationManager.AppSettings[SNAKE_API_KEY];
            _emFlaskApiSercret = ConfigurationManager.AppSettings[SNAKE_API_SECRET];
            _baseUrl = ConfigurationManager.AppSettings[SNAKE_API];
        }

        #endregion

        #region 3. 包装Snake.Api业务的http请求
        private Tuple<bool, T> DoPost<T>(string controller, string action, Dictionary<string, object> paramDic)
        {
            bool success = false;
            string postData = JsonHelper.SerializeObject(paramDic);
            T dataResult = default(T);
            try
            {
                HttpRequestResult httpRequestResult = PostRequest(controller, action, paramDic);
                if (httpRequestResult.Code == 200 && httpRequestResult.Data != null)
                {
                    var dataModel = JsonHelper.DeserializeObject<SnakeApiResponse<T>>(httpRequestResult.Data);
                    if (dataModel.Code == 1)//操作成功
                    {
                        success = true;
                        dataResult = dataModel.Data;
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            return new Tuple<bool, T>(success, dataResult);
        }

        private void DoAsyncPost<T>(string controller, string action, Dictionary<string, object> paramDic, Action<bool, T> finishedHandler)
        {
            string postData = JsonHelper.SerializeObject(paramDic);

            var state = new RequestState();
            PostAsyncRequest(controller, action, paramDic, (httpRequestResult, obj) =>
            {
                try
                {
                    bool isLogicSuccess = false;
                    T dataResult = default(T);
                    if (httpRequestResult.Code == 200 && httpRequestResult.Data != null)
                    {
                        var dataModel = JsonHelper.DeserializeObject<SnakeApiResponse<T>>(httpRequestResult.Data);
                        if (dataModel.Code == 1)//操作成功
                        {
                            isLogicSuccess = true;
                            dataResult = dataModel.Data;
                        }
                    }
                    if (finishedHandler != null)
                    {
                        finishedHandler(isLogicSuccess, dataResult);
                    }
                }
                catch (Exception ex)
                {

                }

            }, state);
        }

        private Tuple<bool, T> DoGet<T>(string controller, string action)
        {
            bool success = false;
            string url = string.Format("{0}/{1}/{2}", _baseUrl, controller, action);
            T dataResult = default(T);
            try
            {
                HttpRequestResult httpRequestResult = GetRequest(controller, action);
                if (httpRequestResult.Code == 200 && httpRequestResult.Data != null)
                {
                    var dataModel = JsonHelper.DeserializeObject<SnakeApiResponse<T>>(httpRequestResult.Data);
                    if (dataModel.Code == 1)//操作成功
                    {
                        success = true;
                        dataResult = dataModel.Data;
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            return new Tuple<bool, T>(success, dataResult);
        }

        #endregion

        #region  4. 包装http请求
        HttpRequestResult PostRequest(string controller, string action, Dictionary<string, object> paramDic)
        {
            string url = string.Format("{0}/{1}/{2}", _baseUrl, controller, action);
            string sign = url.ToLower() + "|" + this._emFlaskApiSercret;
            string secret = EncryptHelper.MD5Encrypt(sign);
            return Post(url, paramDic, (request) =>
            {
                byte[] bytes = Encoding.Default.GetBytes(string.Format("{0}:{1}", this._emFlaskApiKey, secret));
                var ticker = Convert.ToBase64String(bytes);
                //request.Headers.Add("Authorization", string.Format("BasicAuth {0}", ticker));  //.net web api
                request.Headers.Add("Authorization", string.Format("Basic {0}", ticker));  //pyton flask web api
            });
        }

        void PostAsyncRequest(string controller, string action, Dictionary<string, object> paramDic, Action<HttpRequestResult, object> callback, object requestState)
        {
            string url = string.Format("{0}/{1}/{2}", _baseUrl, controller, action);
            string sign = url.ToLower() + "|" + this._emFlaskApiSercret;
            string secret = EncryptHelper.MD5Encrypt(sign);
            PostAsync(url, paramDic, callback, requestState, (request) =>
            {
                byte[] bytes = Encoding.Default.GetBytes(string.Format("{0}:{1}", this._emFlaskApiKey, secret));
                var ticker = Convert.ToBase64String(bytes);
                //request.Headers.Add("Authorization", string.Format("BasicAuth {0}", ticker));
                request.Headers.Add("Authorization", string.Format("Basic {0}", ticker));
            });
        }
        HttpRequestResult GetRequest(string controller, string action)
        {
            string url = string.Format("{0}/{1}/{2}", _baseUrl, controller, action);
            string sign = url.ToLower() + "|" + this._emFlaskApiSercret;
            sign = EncryptHelper.MD5Encrypt(sign);
            return Get(url, (request) =>
            {
                //TODO
                byte[] bytes = Encoding.Default.GetBytes(string.Format("{0}:{1}", this._emFlaskApiKey, sign));
                var ticker = Convert.ToBase64String(bytes);
                request.Headers.Add("Authorization", string.Format("BasicAuth {0}", ticker));
            });
        }

        string AttachParams(Dictionary<string, object> paramDic)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var timestamp = (long)((DateTime.Now - startTime).TotalSeconds);
            paramDic.Add("timespan", timestamp);

            var dic = new Dictionary<string, object>();
            foreach (var dicItem in paramDic)
            {
                dic.Add(dicItem.Key, dicItem.Value);
            }
            // 生成验签
            string sercret = dic.CreatePostSign(this._emFlaskApiSercret);
            return sercret;
        }
        #endregion

        #region SNAKEWEBAPI服务调用代理方法

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trackLog"></param>
        /// <returns></returns>
        public Tuple<bool, T> PublishTrackLog<T>(TrackLogCreatedEvent trackLog)
        {
            Dictionary<string, object> paramDic = trackLog.ToDictionary();
            var action = "PublishTrackLog";
            return DoPost<T>(SNAKE_WEBAPI_TRACKLOG_CONTROLLER, action, paramDic);
        }

        public Tuple<bool, T> PublishAppLog<T>(AppLogCreatedEvent appLog)
        {
            Dictionary<string, object> paramDic = appLog.ToDictionary();
            var action = "PublishAppLog";
            return DoPost<T>(SNAKE_WEBAPI_TRACKLOG_CONTROLLER, action, paramDic);
        }

        public Tuple<bool, T> GetAppLogsPage<T>(PageAppLog appLog)
        {
            Dictionary<string, object> paramDic = appLog.ToDictionary();
            var action = "GetAppLogsPage";
            return DoPost<T>(SNAKE_WEBAPI_TRACKLOG_CONTROLLER, action, paramDic);
        }

        public Task<Tuple<bool, T>> GetAppLogsPageAsync<T>(PageAppLog appLog)
        {
            return Task.Run(() => GetAppLogsPage<T>(appLog));
        }


        public Tuple<bool, T> QueryApplicationOfAppLog<T>(QueryParamDto dto)
        {
            Dictionary<string, object> paramDic = dto.ToDictionary();
            var action = "QueryApplicationOfAppLog";
            return DoPost<T>(SNAKE_WEBAPI_TRACKLOG_CONTROLLER, action, paramDic);
        }

        public Tuple<bool, T> QueryTagsOfAppLog<T>(QueryParamDto dto)
        {
            Dictionary<string, object> paramDic = dto.ToDictionary();
            var action = "QueryTagsOfAppLog";
            return DoPost<T>(SNAKE_WEBAPI_TRACKLOG_CONTROLLER, action, paramDic);
        }

        #endregion
    }


    public class SnakeApiResponse<T>
    {
        public int Code { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }
    }
}
