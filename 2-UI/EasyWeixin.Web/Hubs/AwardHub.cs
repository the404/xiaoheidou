using EasyWeixin.CommonAPIs;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Framework.CommonService;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EasyWeixin.Web.Hubs
{
    [HubName("awardHub")]
    [AllowCrossSiteJson]
    public class AwardHub : Hub
    {
        public AwardHub()
        {
        }

        #region property
        public string GroupId
        {
            //这里使用的是state
            get
            {
                var result = Clients.Caller.userId;
                if (result == null)
                    result = Clients.CallerState.userId;
                if (result == null)
                    result = Context.QueryString["groupId"];
                return result;
            }
        }

        public bool IsMobile
        {
            get
            {
                if (Context.QueryString["mobile"] == "mobile")
                    return true;
                return false;
            }
        }

        public WeixinHelper WeixinHelper
        {
            get { return new WeixinHelper(_qrCodeRepository); }
        }
        #endregion property

        #region 重写这些方法

        public override Task OnConnected()
        {
            Guid other;
            if (!Guid.TryParse(GroupId, out other))
            {
                Clients.Caller.showErrorMessage("你扫描的二维码有误，请刷新电脑网页重新扫描");
                return null;
            }
            var connections = _connections.GetConnections(GroupId);
            _connections.Add(GroupId, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _connections.Remove(GroupId, Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var connections = _connections.GetConnections(GroupId);
            if (!connections.Contains(Context.ConnectionId))
                _connections.Add(GroupId, Context.ConnectionId);
            return base.OnReconnected();
        }

        #endregion 重写这些方法

        #region 定义客户单可调用的方法

        /// <summary>
        /// 发送到pc的消息
        /// </summary>
        /// <param name="openId"></param>
        [HubMethodName("sendPc")]
        public void SendPc()
        {
            var connections = _connections.GetConnections(GroupId).ToList();
            try
            {
                var qrcode = _qrCodeRepository.FindAll().SingleOrDefault(s => s.ID.ToString().ToLower() == GroupId.ToLower());

                if (qrcode == null)
                {
                    Clients.Clients(connections).addErrorMessage("数据不存在,抽奖失败!");
                    return;
                }
                else
                {
                    int num = qrcode.AwardNum;
                    Clients.Clients(connections).startAward(num);

                    //_ts.TraceInformation(DateTime.Now.ToShortTimeString() + ":调用了startAward");
                    //_ts.TraceInformation(DateTime.Now.ToShortTimeString() + ":调用了SendAwardTipTempldateMessage");
                }
            }
            catch (Exception ex)
            {
                Clients.Clients(connections).addErrorMessage(ex.StackTrace);
                LogHelper.WriteException(ex);
            }
        }

        [HubMethodName("sendMobile")]
        public void SendMobile(string url)
        {
            var connections = _connections.GetConnections(GroupId).ToList();
            try
            {
                var qrcode = _qrCodeRepository.FindAll().SingleOrDefault(s => s.ID.ToString().ToLower() == GroupId.ToLower());

                if (qrcode == null)
                {
                    return;
                }

                if (qrcode.AwardNum == 0)
                {
                    Clients.Clients(connections).addErrorMessage("请使用手机扫描二维码");
                    return;
                }

                var openId = qrcode.OpenId;
                var userProfile = qrcode.UserProfile;

                var token = AccessTokenContainer.TryGetToken(userProfile.AppId, userProfile.AppSecret);
                var template = InitTemplateData().Where(s => s.AppId == userProfile.AppId).FirstOrDefault();

                if (template == null)
                {
                    Clients.Clients(connections).addErrorMessage("不存在模板");
                    return;
                }

                WeixinHelper.SendAwardTipTempldateMessage(qrcode, token, openId, url, template.TemplateId);
            }
            catch (Exception ex)
            {
                Clients.Clients(connections).addErrorMessage(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region private method
        private List<TemplateData> InitTemplateData()
        {
            //将每个用户的这个templateId和appId组合写死在这里
            var result = new List<TemplateData>();
            var path = "/Config/templateconfig.json";
            var message = File.ReadAllText(HttpContext.Current.Server.MapPath(path));
            try
            {
                result = JsonConvert.DeserializeObject<List<TemplateData>>(message);
            }
            catch
            {
                throw new Exception("请配置Templateconfig");
            }
            return result;
        }
        #endregion

        #region field

        //private volatile bool _sendingFlag = false;
        private object _sendingLock = new object();

        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        private IQrCodeRepository _qrCodeRepository = Bootstrapper.Initialise().Resolve<IQrCodeRepository>();

        //private TraceSource _ts = LogHelper.AwardHubTs;

        #endregion field
    }

    public class TemplateData
    {
        public TemplateData(string name, string appid, string templateid)
        {
            Name = name;
            AppId = appid;
            TemplateId = templateid;
        }
        public string Name { get; set; }
        public string AppId { get; set; }
        public string TemplateId { get; set; }
    }
}