using System;
using System.Collections.Generic;
using System.Text;
using EasyWeixin.HttpUtility;

namespace EasyWeixin.ShortUrl
{
    /// <summary>
    /// 参数必须是一个?,&后面的自动忽略
    /// </summary>
    public class ShortUrlhelper
    {
        /// <summary>
        /// 记录已经转化过得url
        /// </summary>
        private static Dictionary<string, string> _urls = new Dictionary<string, string>();
        /// <summary>
        /// 文档地址
        /// http://help.baidu.com/question?prod_en=webmaster&class=%CD%F8%D2%B3%CB%D1%CB%F7%CC%D8%C9%AB%B9%A6%C4%DC&id=1000913#05
        /// 5.怎样调用百度短网址API？
        ///生成短网址
        ///请求：向dwz.cn/create.php发送post请求，发送数据包括url=长网址
        ///返回：json格式的数据
        ///status!=0 出错，查看err_msg获得错误信息（UTF-8编码）
        ///成功，返回生成的短网址 tinyurl字段
        /// </summary>
        /// <returns></returns>
        public static string GetShortUrl_Baidu(string longUrl)
        {
            var result = "";
            const string url = "http://dwz.cn/create.php";
            var formData = new Dictionary<string, string> { { "url", longUrl } };
            try
            {
                var shorturlResult = Post.PostGetJson<BaiduShorturlResult>(url, null, formData, Encoding.Default);
                result = shorturlResult.tinyurl;
            }
            catch (Exception)
            {

            }

            return result;
        }

    }

    /// <summary>
    /// {"tinyurl":"http://dwz.cn/1F9Zir","status":0,"longurl":"http://tech.it168.com/d/2008-03-03/200803031616566.shtml","err_msg":""}
    /// </summary>
    public class BaiduShorturlResult
    {
        public string tinyurl { get; set; }
        public string status { get; set; }
        public string longurl { get; set; }
        public string err_msg { get; set; }
    }
}
