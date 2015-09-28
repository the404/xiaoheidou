using Apworks;
using EasyWeixin.Model.RelayRace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class WeixinUser : IAggregateRoot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeixinUserId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public virtual ICollection<WeixinUserInUsers> WeixinUserInUsers { get; set; }

        public virtual ICollection<WeixinUserInActivity> WeixinUserInActivities { get; set; }

        public virtual ICollection<MainRace> MainRaces { get; set; }

        public virtual ICollection<SubRace> SubRaces { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime UpdateDate { get; set; }

        #region 微信用户基本信息来源于 CommonApi.GetUserInfo()

        /// <summary>
        ///     用户是否订阅该公众号标识，值为0时，拉取不到其余信息
        /// </summary>
        public int Subscribe { get; set; }

        /// <summary>
        ///     普通用户的标识，对当前公众号唯一
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        ///     城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///     省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        ///     国籍
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     订阅时间
        /// </summary>
        public int SubscribeTime { get; set; }

        public string Remark { get; set; }

        /// <summary>
        ///     普通用户的昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        ///     普通用户的头像链接
        /// </summary>
        public string Headimgurl { get; set; }

        /// <summary>
        ///     普通用户的语言，简体中文为zh_CN
        /// </summary>
        public string Language { get; set; }

        public string Unionid { get; set; }

        public string Privilege { get; set; }

        #endregion 微信用户基本信息来源于 CommonApi.GetUserInfo()
    }
}