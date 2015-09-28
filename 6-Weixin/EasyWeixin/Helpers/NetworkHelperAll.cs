using System;
using System.IO;
using System.Net;
using System.Text;

namespace EasyWeixin.Helpers
{
    /// <summary>
    /// 网络有关处理函数
    /// </summary>
    public class NetworkHelperAll
    {
        private static HttpWebRequest request;
        private static HttpWebResponse response;

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
                wreq.Method = "POST";
                wreq.ContentType = "application/x-www-form-urlencoded";

                wrep = wreq.GetResponse();
                stream = wrep.GetResponseStream();

                reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("GB2312"));
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
                //if (wrep != null) wrep.Dispose();
                if (wreq != null) wreq.Abort();
            }
            return data;
        }

        /// <summary>
        /// post提交
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpPost(string Url, string Sign, string Timesp, string Non, string postXml)
        {
            ServicePointManager.DefaultConnectionLimit = 500;
            string postData = string.Format("?Sign={0}&Timesp={1}&Non={2}", Sign, Timesp, Non); // 其他参数
            //string postData = "?Sign=58&Timesp=656&Non=5453";
            //请求
            //request = (HttpWebRequest)WebRequest.Create(Url +postData);
            //request.Method = "POST";
            //request.ContentType = "application/xml";
            //request.Timeout = 200000;
            //request.ContentLength = Encoding.UTF8.GetByteCount(postXml);

            //string path = System.Web.HttpContext.Current.Server.MapPath("/txt/");
            //string ss = "cc.txt";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            //StreamWriter sw = new StreamWriter(path + ss, true, Encoding.UTF8);
            //sw.WriteLine(request.ContentLength);
            //sw.Close();

            //Stream myRequestStream = request.GetRequestStream();
            //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            //myStreamWriter.Write(postXml);
            //myStreamWriter.Flush();
            //myStreamWriter.Close();

            ////读取
            //response = (HttpWebResponse)request.GetResponse();
            //Stream myResponseStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            //string retString = myStreamReader.ReadToEnd();
            //myStreamReader.Close();
            //myResponseStream.Close();

            //return retString;

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(Url + postData);
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.ContentType = "application/xml";

            byte[] encodedBytes = Encoding.UTF8.GetBytes(postXml);
            myHttpWebRequest.ContentLength = encodedBytes.Length;

            Stream requestStream = myHttpWebRequest.GetRequestStream();
            requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            requestStream.Close();

            HttpWebResponse result;

            try
            {
                result = (HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("/txt/");
                string ss = "cc.txt";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                StreamWriter sw = new StreamWriter(path + ss, true, Encoding.UTF8);
                sw.WriteLine(e.InnerException);
                sw.Close();
                return string.Empty;
            }

            if (result.StatusCode == HttpStatusCode.OK)
            {
                using (Stream mystream = result.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(mystream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr)
        {
            request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string Post_Data(string url, string postdata)
        {
            string temp = null;
            Encoding encod = Encoding.GetEncoding("gb2312");
            try
            {
                byte[] byteArray = encod.GetBytes(postdata); // 转化
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; InfoPath.1)";
                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                StreamReader str = new StreamReader(response.GetResponseStream(), encod);
                temp = str.ReadToEnd();
            }
            catch (Exception ex)
            {
                temp = ex.ToString();
            }
            return temp;
        }
    }
}