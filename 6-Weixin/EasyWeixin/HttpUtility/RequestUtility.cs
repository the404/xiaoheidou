using EasyWeixin.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EasyWeixin.HttpUtility
{
    public static class RequestUtility
    {
        /// <summary>
        /// 使用Get方法获取字符串结果（暂时没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encoding = null)
        {
            WebClient wc = new WebClient();
            if (encoding != null)
            {
                wc.Encoding = encoding;
            }
            return wc.DownloadString(url);
        }

        public static string HttpPost(string url, string cookieStr, string formStr, Encoding encoding = null)
        {
            var cc = SetCookie(cookieStr, new Uri(url).Host);
            var formData = SplitStr(formStr);
            return HttpPost(url, cc, formData, encoding);
        }

        private static Dictionary<string, string> SplitStr(string cookieStr)
        {
            if (cookieStr.EndsWith(";"))//因为最后的;会导致分组后多出一个空字符串组
            {
                cookieStr = cookieStr.Substring(0, cookieStr.Length - 1);
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] strs = cookieStr.Split(';');
            foreach (var item in strs)
            {
                try
                {
                    string[] items = item.Split('=');
                    dic.Add(items[0].Trim(), items[1].Trim());
                }
                catch
                {
                    throw new ArgumentException("格式错误,键值对必须以=连接");
                }
            }
            return dic;
        }

        private static CookieContainer SetCookie(string cookieStr, string url = "oneplusbbs.com")
        {
            var dic = SplitStr(cookieStr);
            return SetCookie(dic, url);
        }

        private static CookieContainer SetCookie(Dictionary<string, string> dic, string url = "oneplusbbs.com")
        {
            CookieContainer cc = new CookieContainer();
            foreach (var item in dic)
            {
                cc.Add(new Cookie(item.Key, item.Value, "", url));
            }
            return cc;
        }

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            MemoryStream ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            return HttpPost(url, cookieContainer, ms, false, encoding);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="fileName">requestStream中的数据</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, string fileName = null, Encoding encoding = null)
        {
            //读取文件
            var fileStream = FileHelper.GetFileStream(fileName);
            return HttpPost(url, cookieContainer, fileStream, true, encoding);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postStream">要发送的数据流</param>
        /// <param name="isFile">postStreams是否是文件流</param>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, Stream postStream = null, bool isFile = false, Encoding encoding = null)
        {
            var result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.ServicePoint.Expect100Continue = false;

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }

            if (postStream != null)
            {
                //postStream.Position = 0;

                //上传文件流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                postStream.Close();//关闭文件访问
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }
            try
            {
                //不要这样写,因为有些返回值contentleng为-1,但是他仍然是有返回值的
                //为了保险我宁愿使用trycatch 不处理异常,但可以添加日志,没有空
                //if (response.ContentLength > 0)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (
                            StreamReader myStreamReader = new StreamReader(responseStream,
                                encoding ?? Encoding.GetEncoding("utf-8")))
                        {
                            result = myStreamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return result;
        }

        /// <summary>
        /// 请求是否发起自微信客户端的浏览器
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool IsWeixinClientRequest(this HttpContext httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.Request.UserAgent) &&
                   httpContext.Request.UserAgent.Contains("MicroMessenger");
        }

        /// <summary>
        /// 组装QueryString的方法
        /// 参数之间用&连接，首位没有符号，如：a=1&b=2&c=3
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlEncode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string html)
        {
            return System.Web.HttpUtility.HtmlEncode(html);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlDecode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string html)
        {
            return System.Web.HttpUtility.HtmlDecode(html);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.UrlEncode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.UrlDecode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }

        public static string ToUnicode(string str)
        {
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2) r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }
        /// <summary>
        /// 将Unicode编码转换为汉字字符串
        /// </summary>
        /// <param name="str">Unicode编码字符串</param>
        /// <returns>汉字字符串</returns>
        public static string ToGB2312(string str)
        {
            string r = "";
            MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                r += Encoding.Unicode.GetString(bts);
            }
            return r;
        }
    }
}