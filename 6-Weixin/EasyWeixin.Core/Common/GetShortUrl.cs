using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace EasyWeixin.Core.Common
{
    public class GetShortUrl
    {
        public static string GetSina(string urlLong)
        {
            var result = "";
            try
            {
                string url = "http://api.t.sina.com.cn/short_url/shorten.json?source=1681459862&url_long=" + urlLong;
                string jsonstring = NetworkHelper.SendRequest(url);
                if (jsonstring.Contains("url_short"))
                {
                    result =
                        JsonConvert.DeserializeObject<System.Collections.Generic.List<SinaShortUrl>>(
                            jsonstring.Replace("\r\n", ""))[0].url_short;
                }
            }
            catch (Exception)
            {

            }
            return result;
        }
    }



    public class SinaShortUrl
    {
        public string url_short { get; set; }

        public string url_long { get; set; }

        public string type { get; set; }

        //[{"url_short":"http://t.cn/z8Fg6sb","url_long":"http://" + Request.Url.Host +"/News/GuessNews?ImageTextID=44d808e0-ab83-4642-97c2-67c58991a29f","type":0}]
    }

    public sealed class Base64
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encode">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            string result = "";
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encode">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="source">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string source)
        {
            string result = "";
            byte[] bytes = Convert.FromBase64String(source);
            try
            {
                result = encode.GetString(bytes);
            }
            catch
            {
                result = result;
            }
            return result;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
    }
}

