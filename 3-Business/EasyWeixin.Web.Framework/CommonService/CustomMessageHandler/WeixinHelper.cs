using EasyWeixin.AdvancedAPIs.TemplateMessage;
using EasyWeixin.AdvancedAPIs.TemplateMessage.TemplateMessageJson;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.Model;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EasyWeixin.Web.Framework.CommonService
{
    /// <summary>
    /// 定义通用的微信方法
    /// </summary>
    public class WeixinHelper
    {
        private volatile bool _updatingFlag = false;
        private object _updatingLock = new object();
        private IQrCodeRepository _qrCodeRepository;

        public WeixinHelper(IQrCodeRepository qrCodeRepo)
        {
            _qrCodeRepository = qrCodeRepo;
        }

        /// <summary>
        /// 发送模板消息的一个封装
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="openId"></param>
        /// <param name="templateId"></param>
        public void SendAwardTipTempldateMessage(QrCode qrCode,
            string accessToken, string openId, string url,
            string templateId)
        {
            #region
            lock (_updatingLock)
            {
                if (!_updatingFlag)
                {
                    try
                    {
                        _updatingFlag = true;

                        if (qrCode.IsWeixinSend) return; //保证只发送一条提示信息

                        var str = GetKudo(qrCode.AwardNum);

                        var testData = new
                        {
                            first = new TemplateDataItem(string.Format("恭喜你获得了{0}奖励", str)),
                            keyword1 = new TemplateDataItem(DateTime.Now.ToString()),
                            keyword2 = new TemplateDataItem(str),
                        };

                        var result = TemplateApi.SendTemplateMessage(accessToken, openId, templateId,
                            "#FF0000", url, testData);
                        LogHelper.WriteLog(string.Format("a:  {0}  ,  b: {1}  , t:  {2} ,u:  {3},  data:{4}", accessToken, openId, templateId, url, JsonConvert.SerializeObject(testData)), LogHelper.LogMark);
                        qrCode.IsWeixinSend = true;
                        _qrCodeRepository.Update(qrCode);
                        _qrCodeRepository.Context.Commit();
                        _updatingFlag = false;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteException(ex);
                    }
                }
            }
            #endregion
        }

        private string GetKudo(int awardNum)
        {
            var result = "";
            //todo 有可能这些个奖品是来源于后台设置的,所以先这样封装出一个方法吧
            switch (awardNum)
            {
                case 1:
                    result = "100积分";
                    break;

                case 2:
                    result = "一等奖";
                    break;

                case 3:
                    result = "三等奖";
                    break;

                case 4:
                    result = "50积分";
                    break;

                case 5:
                    result = "100积分";
                    break;

                case 6:
                    result = "二等奖";
                    break;

                case 7:
                    result = "150积分";
                    break;

                case 8:
                    result = "三等奖";
                    break;

                case 9:
                    result = "50积分";
                    break;

                case 10:
                    result = "三等奖";
                    break;

                case 11:
                    result = "二等奖";
                    break;

                case 12:
                    result = "50积分";
                    break;
            }
            return result;
        }

        public void UpdateQrCodeStatu(string eventKey, string openId)
        {
            var qrCode = GetQrCode(eventKey);
            if (qrCode == null)
                return;
            if (qrCode.IsWeixinSend)
                return;

            Random random = new Random(DateTime.Now.Millisecond);
            qrCode.AwardNum = random.Next(1, 12);
            qrCode.OpenId = openId;
            _qrCodeRepository.Update(qrCode);
            _qrCodeRepository.Context.Commit();

            var url = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/";

            EstablishConnection(qrCode.ID.ToString(), openId, url);
        }

        public QrCode GetQrCode(string eventKey)
        {
            var qrCode = _qrCodeRepository.FindAll().Where(s => s.ExpireTime > DateTime.Now).SingleOrDefault(
              s => eventKey == s.SceneId ||
                  eventKey == "qrscene_" + s.SceneId);

            return qrCode;
        }

        /// <summary>
        /// 建立连接后调用sendClient方法
        /// </summary>
        /// <param name="groupId"></param>
        private static void EstablishConnection(string groupId, string openId, string url)
        {
            Dictionary<string, string> qs = new Dictionary<string, string>();
            qs.Add("groupId", groupId);
            qs.Add("mobile", "mobile");
            HubConnection hubConn = new HubConnection(url, qs);
            IHubProxy awardHub = hubConn.CreateHubProxy("awardHub");

            hubConn.Start().ContinueWith(task =>
            {
                awardHub.Invoke("sendPc");
            });
        }

        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;

                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);
                //request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
    }
}