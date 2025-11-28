using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Snake.Core.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            string ip = string.Empty;

            try
            {
                // Web-hosting. Needs reference to System.Web.dll
                //if (request.Properties.ContainsKey(HttpContext))
                //{
                //    dynamic ctx = request.Properties[HttpContext];
                //    if (ctx != null && ctx.Request != null)
                //    {
                //        ip = ctx.Request.UserHostAddress;
                //    }

                //}
                // Self-hosting. Needs reference to System.ServiceModel.dll. 
                if (request.Properties.ContainsKey(RemoteEndpointMessage))
                {
                    dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                    if (remoteEndpoint != null && remoteEndpoint.Address != null)
                    {
                        ip = remoteEndpoint.Address;
                    }
                }
                // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
                else if (request.Properties.ContainsKey(OwinContext))
                {
                    dynamic owinContext = request.Properties[OwinContext];
                    if (owinContext != null && owinContext.Request.RemoteIpAddress != null)
                    {
                        ip = owinContext.Request.RemoteIpAddress;
                    }
                }

                if (ip == "::1") ip = "127.0.0.1";

            }
            catch (Exception)
            {
                ip = string.Empty;
            }

            if (string.IsNullOrEmpty(ip) || !IsIPAddress(ip))
                return "127.0.0.1";
            else
                return ip;
        }

        #region  判断是否是IP格式

        ///  <summary>
        ///  判断是否是IP地址格式  0.0.0.0
        ///  </summary>
        ///  <param  name="str1">待判断的IP地址</param>
        ///  <returns>true  or  false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15) return false;

            const string regFormat = @"^d{1,3}[.]d{1,3}[.]d{1,3}[.]d{1,3}$";

            var regex = new Regex(regFormat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        #endregion

    }
}
