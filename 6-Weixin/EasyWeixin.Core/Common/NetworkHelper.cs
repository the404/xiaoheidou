using System;
using System.IO;
using System.Net;
using System.Web;

namespace EasyWeixin.Core.Common
{
    /// <summary>
    /// 网络有关处理函数
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// 向服务器发送请求并返回请求结果
        /// </summary>
        /// <param name="url">请求url</param>
        /// <returns>返回Response结果</returns>
        public static string SendRequest(string url)
        {
            string data = string.Empty;
            WebRequest wreq = null;
            WebResponse wrep = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                wreq = WebRequest.Create(url);
                wrep = wreq.GetResponse();
                stream = wrep.GetResponseStream();
                reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                data = reader.ReadToEnd();
            }
            catch (Exception)
            {
                //throw;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (wrep != null) wrep.Dispose();
                if (wreq != null) wreq.Abort();
            }
            return data;
        }

        /// <summary>
        /// 穿透代理服务器获取真实ip
        /// </summary>
        /// <returns></returns>
        public static string GetRealyIp()
        {
            string Ip = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                {
                    if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                        Ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                    else
                        if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                        Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    else
                        Ip = string.Empty;
                }
                else
                    Ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                Ip = string.Empty;
            }
            int index = Ip.IndexOf(',');
            if (index > 0)
            {
                Ip = Ip.Substring(0, index);
            }
            return Ip;
        }
    }
}