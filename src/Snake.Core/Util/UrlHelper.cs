using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Snake.Core.Util
{
    public static class UrlHelper
    {
        /// <summary>
        /// Gets the parameters from the url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IDictionary<string, string> GetParameters(string url)
        {
            var parms = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(url))
            {
                int questionMarkIndex = url.IndexOf('?');
                string parametersString;
                if (questionMarkIndex > 0 && questionMarkIndex < url.Length)
                {
                    parametersString = url.Substring(questionMarkIndex + 1);
                }
                else
                {
                    parametersString = url;
                }

                var regex = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.IgnoreCase);
                MatchCollection mc = regex.Matches(parametersString);

                foreach (Match m in mc)
                {
                    parms.Add(m.Result("$2"), m.Result("$3"));
                }
            }

            return parms;
        }

        /// <summary>
        /// Encodes the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private static string EncodeParameter(string parameter)
        {
            //var val = parameter.Replace("+", "%2B");
            //val = val.Replace("&", "%26");
            var val = string.IsNullOrEmpty(parameter) ? Uri.EscapeDataString("") : Uri.EscapeDataString(parameter);
            //var val = parameter;
            return val;
        }

        /// <summary>
        /// Escapes the data format.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public static string EscapeDataFormat(string url, string parameter)
        {
            return string.Format(url, EncodeParameter(parameter));
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetParameter(string url, string key)
        {
            var parms = GetParameters(url);

            if (parms.ContainsKey(key))
            {
                return parms[key];
            }

            return string.Empty;
        }

        public static string GetString(this Uri uri, UrlPart urlPart)
        {
            var url = uri.OriginalString;

            if (!string.IsNullOrEmpty(url))
            {
                int questionMarkIndex = url.IndexOf('?');
                if (urlPart == UrlPart.FullPath)
                {
                    return url;
                }
                else if (urlPart == UrlPart.BasePath)
                {
                    if (questionMarkIndex != -1)
                    {
                        return url.Substring(0, questionMarkIndex);
                    }
                    else
                    {
                        return url;
                    }

                }
                else if (urlPart == UrlPart.QueryString)
                {
                    if (questionMarkIndex != -1 && url.Length > questionMarkIndex + 1)
                    {
                        return url.Substring(questionMarkIndex + 1, url.Length - questionMarkIndex - 1);
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Appends the parameters to the url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static string AppendParameters(string url, IDictionary<string, string> parameters)
        {
            //remove old parameters..
            url = RemoveParameters(url, parameters);

            var sb = new StringBuilder();
            var firstchar = url.IndexOf("?") == -1 ? "?" : "&";
            var i = 0;
            foreach (var param in parameters)
            {
                const string splitchar = "&";
                sb.Append(string.Format("{0}{1}={2}", i == 0 ? firstchar : splitchar, param.Key, EncodeParameter(param.Value)));

                i++;
            }

            return string.Concat(url, sb.ToString());
        }

        /// <summary>
        /// Removes the parameters.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static string RemoveParameters(string url, IDictionary<string, string> parameters)
        {
            var urlarray = url.Split('?');
            if (urlarray.Length == 2)
            {
                var paramarray = urlarray[1].Split('&');
                var sb = new StringBuilder();

                foreach (var p in paramarray)
                {
                    var parray = p.Split('=');

                    var needtoremove = false;
                    foreach (var dicitem in parameters)
                    {
                        if (string.Compare(parray[0], dicitem.Key, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            needtoremove = true;
                            break;
                        }
                    }

                    if (!needtoremove)
                    {
                        sb.Append(String.Concat("&", p));
                    }
                }

                return string.Format("{0}?{1}", urlarray[0], sb.ToString().TrimStart('&'));
            }

            return url;
        }

        private static IEnumerable<IPAddress> GetIPs()
        {
            IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
            return addressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }

        public static string GetIPv4()
        {
            IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
            var ipv4 = addressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();
            return ipv4.ToString(); ;
        }

        public static string GetIPv6()
        {
            IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
            var ipv6 = addressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6).FirstOrDefault();
            return ipv6.ToString();
        }

        /// <summary>
        /// scheme://host:port/path?query#fragment
        /// </summary>
        public enum UrlPart
        {
            BasePath,
            FullPath,
            QueryString
        }
    }
}
