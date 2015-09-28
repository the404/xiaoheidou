using EasyWeixin.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.Entities.JsonResult
{
    public class WeixinShortLinkSend
    {
        public WeixinShortLinkSend()
        {
            action = "long2short";
        }
        public string access_token { get; set; }
        public string action { get; private set; }
        public string long_url { get; set; }
    }
    public class WeixinShortLinkResult
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string short_url { get; set; }
    }
}
