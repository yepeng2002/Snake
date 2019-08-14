using System;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Snake.Core.Util;
using System.Net.Http;

namespace Snake.Api.Controllers
{
    public class SnakeBasicAuthorizeAttribute : AuthorizeAttribute
    {
        const string EnableSignAuthorize_KEY = "SnakeEnableSignAuthorize";
        const string SNAKE_API_KEY = "SNAKE_API";
        const string SNAKE_API_SECRET_KEY = "SNAKE_API_SECRET";

        const string AuthorizeTimeSpanKey = "AuthorizeTimeSpan";
        const string TimeSpanKey = "timespan";
        static readonly TimeSpan DefaultTimeSpan = new TimeSpan(4, 0, 0);

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                string userInfo = Encoding.Default.GetString(Convert.FromBase64String(actionContext.Request.Headers.Authorization.Parameter));
                //使用新的验签的方式验证
                EmSignAuthorization(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        /// <summary>
        /// 重写用户授权失败，添加相关的验证信息
        /// </summary>
        /// <param name="message">验证信息</param>
        private void HandleUnauthorizedRequest(HttpActionContext actionContext, string message)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");

            throw new Exception(message);
        }

        private void EmSignAuthorization(HttpActionContext actionContext)
        {
            bool enableSignAuthorize = bool.Parse(ConfigurationManager.AppSettings[EnableSignAuthorize_KEY]);
            if (!enableSignAuthorize)
            {
                IsAuthorized(actionContext);
                return;
            }

            if (actionContext.Request.Headers.Authorization != null)
            {
                var url = actionContext.Request.RequestUri.AbsoluteUri;
                url = System.Web.HttpUtility.UrlDecode(url);
                string sign = string.Empty;

                var hzKey = ConfigurationManager.AppSettings[SNAKE_API_KEY];
                var hzSecret = ConfigurationManager.AppSettings[SNAKE_API_SECRET_KEY];
                //url需要全部转成小写，前端是大写，API端获取到的url是小写，大小写不一致导致加密后不匹配                
                sign = EncryptHelper.MD5Encrypt(url.ToLower() + "|" + hzSecret);

                string userInfo = Encoding.Default.GetString(Convert.FromBase64String(actionContext.Request.Headers.Authorization.Parameter));
                var fields = userInfo.Split(':');
                if (fields.Length < 2)
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }

                var checkUserID = fields[0];
                var checkSign = fields[1];
                if (string.Equals(hzKey, checkUserID) && string.Equals(sign, checkSign))
                {
                    IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
            throw new System.Web.Http.HttpResponseException(challengeMessage);
        }
    }
}