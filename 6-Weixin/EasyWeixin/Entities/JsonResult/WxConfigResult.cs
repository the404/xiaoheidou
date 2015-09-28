namespace EasyWeixin.Entities.JsonResult
{
    public class WxConfigResult
    {
        public string url { get; set; }

        /// <summary>
        /// "你的AppID",
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 生成的时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string noncestr { get; set; }

        /// <summary>
        /// 使用sha1加密的签名
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// ['onMenuShareTimeline', 'onMenuShareAppMessage'] // 功能列表，我们要使用JS-SDK的什么功能
        /// </summary>
        public string jsapilist { get; set; }

        public string ticket { get; set; }
    }
}