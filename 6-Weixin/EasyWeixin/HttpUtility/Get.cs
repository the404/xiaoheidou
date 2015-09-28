using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Exceptions;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace EasyWeixin.HttpUtility
{
    public static class Get
    {
        public static T GetJson<T>(string url, Encoding encoding = null)
        {
            string returnText = RequestUtility.HttpGet(url, encoding);

            //JavaScriptSerializer js = new JavaScriptSerializer();

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                WxJsonResult errorResult = JsonConvert.DeserializeObject<WxJsonResult>(returnText);
                if (errorResult.errcode != ReturnCode.请求成功)
                {
                    //发生错误
                    throw new ErrorJsonResultException(
                        string.Format("微信请求发生错误！错误代码：{0}，说明：{1}",
                                        (int)errorResult.errcode,
                                        errorResult.errmsg),
                                      null, errorResult);
                }
            }

            T result = JsonConvert.DeserializeObject<T>(returnText);

            return result;
        }

        public static void Download(string url, Stream stream)
        {
            WebClient wc = new WebClient();
            var data = wc.DownloadData(url);
            foreach (var b in data)
            {
                stream.WriteByte(b);
            }
        }
    }
}