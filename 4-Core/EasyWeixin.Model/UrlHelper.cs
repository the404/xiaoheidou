using System.Collections.Generic;
using System.Web;

namespace EasyWeixin.Model
{
    /// <summary>
    /// 用于对硬编码的数据进行替换
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// 这是为了解决域名迁移的问题
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ReplaceHost(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            //所有需要替换的域名
            List<string> list = new List<string>()
            {
                "http://weixin.ipow.cn",
                "http://weixin.happyvalley.cn",
                "http://wxt.happyvalley.cn"
            };
            var newstr = "http://" + HttpContext.Current.Request.Url.Authority;
            foreach (var item in list)
            {
                var oldstr = item;
                if (url.Contains(oldstr))
                {
                    url = url.Replace(oldstr, newstr);
                    break;
                }
            }
            return url;
        }
    }
}