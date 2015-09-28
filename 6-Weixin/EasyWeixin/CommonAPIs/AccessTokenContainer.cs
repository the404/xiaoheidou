using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Exceptions;
using System;
using System.Collections.Generic;

namespace EasyWeixin.CommonAPIs
{
    internal class AccessTokenBag
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public DateTime ExpireTime { get; set; }

        public AccessTokenResult AccessTokenResult { get; set; }

        /// <summary>
        /// 只针对这个AppId的锁
        /// </summary>
        public object Lock = new object();
    }

    /// <summary>
    /// 通用接口AccessToken容器，用于自动管理AccessToken，如果过期会重新获取
    /// </summary>
    public class AccessTokenContainer
    {
        private static readonly Dictionary<string, AccessTokenBag> AccessTokenCollection =
            new Dictionary<string, AccessTokenBag>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 注册应用凭证信息，此操作只是注册，不会马上获取Token，并将清空之前的Token，
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        public static void Register(string appId, string appSecret)
        {
            if (string.IsNullOrEmpty(appId))
                throw new WeixinException("微信公众号APPId未设置,请点击菜单个人信息管理->微信信息中设置");
            if (string.IsNullOrEmpty(appSecret))
                throw new WeixinException("微信公众号AppSecret未设置,请点击菜单个人信息管理->微信信息中设置");

            var bag = new AccessTokenBag()
            {
                AppId = appId,
                AppSecret = appSecret,
                ExpireTime = DateTime.MinValue,
                AccessTokenResult = new AccessTokenResult()
            };
            if (!AccessTokenCollection.ContainsKey(appId))
                AccessTokenCollection.Add(appId, bag);
            else
                AccessTokenCollection[appId] = bag;
        }

        /// <summary>
        /// 使用完整的应用凭证获取Token，如果不存在将自动注册
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        public static string TryGetToken(string appId, string appSecret, bool getNewToken = false)
        {
            if (!CheckRegistered(appId) || getNewToken)
            {
                Register(appId, appSecret);
            }
            return GetToken(appId);
        }

        /// <summary>
        /// 获取可用Token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNewToken">是否强制重新获取新的Token</param>
        /// <returns></returns>
        public static string GetToken(string appId, bool getNewToken = false)
        {
            return GetTokenResult(appId, getNewToken).access_token;
        }

        /// <summary>
        /// 获取可用Token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNewToken">是否强制重新获取新的Token</param>
        /// <returns></returns>
        public static AccessTokenResult GetTokenResult(string appId, bool getNewToken = false)
        {
            if (!AccessTokenCollection.ContainsKey(appId))
            {
                throw new WeixinException("此appId尚未注册，请先使用AccessTokenContainer.Register完成注册（全局执行一次即可）！");
            }

            var accessTokenBag = AccessTokenCollection[appId];
            lock (accessTokenBag.Lock)
            {
                if (getNewToken || accessTokenBag.ExpireTime <= DateTime.Now)
                {
                    //已过期，重新获取
                    accessTokenBag.AccessTokenResult = CommonApi.GetToken(accessTokenBag.AppId, accessTokenBag.AppSecret);
                    accessTokenBag.ExpireTime = DateTime.Now.AddSeconds(accessTokenBag.AccessTokenResult.expires_in - 1000);
                }
            }
            return accessTokenBag.AccessTokenResult;
        }

        /// <summary>
        /// 检查是否已经注册
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static bool CheckRegistered(string appId)
        {
            if (string.IsNullOrEmpty(appId))
                throw new WeixinException("微信公众号APPId未设置,请点击菜单个人信息管理->微信信息中设置");
            return AccessTokenCollection.ContainsKey(appId);
        }
    }
}