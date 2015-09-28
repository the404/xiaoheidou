using EasyWeixin.AdvancedAPIs.QrCode;
using EasyWeixin.CommonAPIs;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Areas.Activity.Models;
using EasyWeixin.Web.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Areas.Activity.Controllers
{
    public class QrCodeController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IQrCodeRepository _qrCodeRepository;
        private readonly object _object = new object();

        public QrCodeController(
            IUserProfileRepository userProfileRepository,
            IQrCodeRepository qrCodeRepository)
        {
            _userProfileRepository = userProfileRepository;
            _qrCodeRepository = qrCodeRepository;
        }

        public ActionResult Index(Guid? userGuid)
        {
            if (userGuid == null)
                userGuid = Guid.Parse("{622A17D7-355A-4D59-AF18-416C3BFBA132}");
            var userProfile = _userProfileRepository.FindAll().SingleOrDefault(s => s.ID == userGuid);
            if (userProfile == null)
                return View();
            var qrCode = AddQrCode(userProfile);
            OrCodeViewModel qrCodeViewModel = new OrCodeViewModel
            {
                QrCodeUrl = qrCode.QrCodeUrl,
                UserName = userProfile.UserName,
                SceneId = qrCode.SceneId
            };

            return View(qrCodeViewModel);
        }

        /// <summary>
        /// 公开接口,调用的时候可以添加一个新的二维码，翻译的数据包括二维码地址和userid
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [JsonpFilter]
        public JsonResult GetQrCode(Guid? userGuid, string callback)
        {
            if (userGuid == null)
                userGuid = Guid.Parse("{622A17D7-355A-4D59-AF18-416C3BFBA132}");
            var userProfile = _userProfileRepository.FindAll().SingleOrDefault(s => s.ID == userGuid);
            if (userProfile == null)
                throw new Exception("用户不存在");
            var qrCode = AddQrCode(userProfile);
            var result = new { Num = qrCode.SceneId, Url = qrCode.QrCodeUrl, GroupId = qrCode.ID };
            return Json(result);
        }

        /// <summary>
        /// 根据公众号用户信息，添加相应的QrCode
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        private QrCode AddQrCode(UserProfile userProfile)
        {
            lock (_object)
            {
                var expireSpan = new TimeSpan(0, 12, 0); //过期时间
                var token = AccessTokenContainer.TryGetToken(userProfile.AppId, userProfile.AppSecret);
                var sceneId = "1" + DateTime.Now.ToString("HHmmssff");
                var createQrCodeResult = QrCodeApi.Create(token, sceneId, Convert.ToInt32(expireSpan.TotalSeconds));
                var qrcodeUrl = QrCodeApi.GetShowQrCodeUrl(createQrCodeResult.ticket);


                #region 保存二维码

                if (!string.IsNullOrEmpty(qrcodeUrl))
                {
                    var qrCode = new QrCode
                    {
                        SceneId = sceneId,
                        UserId = userProfile.UserId,
                        Type = OrCodeType.Award,
                        QrCodeUrl = qrcodeUrl,
                        ExpireTime = DateTime.Now.Add(expireSpan)
                    };

                    _qrCodeRepository.Add(qrCode);
                    _qrCodeRepository.Context.Commit();
                    return qrCode;
                }

                #endregion

                throw new Exception("生成二维码失败");
            }
        }
    }
}