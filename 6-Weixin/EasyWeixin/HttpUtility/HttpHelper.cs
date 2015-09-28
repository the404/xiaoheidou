using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.HttpUtility
{
    public class HttpHelper
    {
        /// <summary>
        /// 创建GET方式的HTTP请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgent"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout, string userAgent,
            CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性检验(非第三方权威机构颁发的证书,如自己生成的,不进行验证,这里返回true)
                ServicePointManager.ServerCertificateValidationCallback =
                    new System.Net.Security.RemoteCertificateValidationCallback(CheckValidateionResult);
                request.ProtocolVersion = HttpVersion.Version11;
            }
            request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = userAgent;
            request.Method = "GET";
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters,
            int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性检验(非第三方权威机构颁发的证书,如自己生成的,不进行验证,这里返回true)
                ServicePointManager.ServerCertificateValidationCallback =
                    new System.Net.Security.RemoteCertificateValidationCallback(CheckValidateionResult);
                request.ProtocolVersion = HttpVersion.Version11;
            }
            request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = userAgent;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = timeout;

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            if (!(parameters == null) || parameters.Count() == 0)
            {
                StringBuilder buffer = new StringBuilder();
                bool flag = false;
                foreach (string key in parameters.Keys)
                {
                    if (flag)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        flag = true;
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            return request.GetResponse() as HttpWebResponse;
        }

        public static string GetResponseString(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                return sr.ReadToEnd();
            }
        }
        private static bool CheckValidateionResult(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (SslPolicyErrors.None == sslPolicyErrors)
                return true;
            return false;
        }
    }
}
