/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc

    文件名：TemplateAPI.cs
    文件功能描述：模板消息接口

    创建标识：Senparc - 20150211

    修改标识：Senparc - 20150303
    修改描述：整理接口

    修改标识：Senparc - 20150312
    修改描述：开放代理请求超时时间
----------------------------------------------------------------*/

/*
    API：http://mp.weixin.qq.com/wiki/17/304c1885ea66dbedf7dc170d84999a9d.html
 */

using EasyWeixin.AdvancedAPIs.TemplateMessage.TemplateMessageJson;
using EasyWeixin.CommonAPIs;

namespace EasyWeixin.AdvancedAPIs.TemplateMessage
{
    /// <summary>
    /// 模板消息接口
    /// </summary>
    public static class TemplateApi
    {
        /// <summary>
        /// 模板消息接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="templateId"></param>
        /// <param name="topcolor"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static SendTemplateMessageResult SendTemplateMessage(string accessToken, string openId, string templateId, string topcolor, string url, object data, int timeOut = Config.TIME_OUT)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            var msgData = new TempleteModel()
            {
                touser = openId,
                template_id = templateId,
                topcolor = topcolor,
                url = url,
                data = data
            };
            return CommonJsonSend.Send<SendTemplateMessageResult>(accessToken, urlFormat, msgData);
        }

        public static string GetTemplateId(string appId, string appSecret, string shortTemplateId)
        {
            var accessToken = AccessTokenContainer.TryGetToken(appId, appSecret);
            string url = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";

            var msgData = new
            {
                template_id_short = shortTemplateId
            };

            return CommonJsonSend.Send<GetTemplateIdResult>(accessToken, url, msgData).template_id_short;
        }

        public static void SetIndustry(string appId, string appSecret)
        {
            var accessToken = AccessTokenContainer.TryGetToken(appId, appSecret);
            //string url = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token=ACCESS_TOKEN";
        }
    }
}