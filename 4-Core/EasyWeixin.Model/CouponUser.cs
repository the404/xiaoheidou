using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class CouponUser : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CouponUserID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string CouponUserName { get; set; }

        public string Identification { get; set; }

        public string CouponUserEmail { get; set; }

        public string CouponUserPhone { get; set; }

        public string CouponUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        /// <summary>
        /// 获奖者真实姓名
        /// </summary>
        public string CouponTrueName { get; set; }

        /// <summary>
        /// 获取的SN编号
        /// </summary>
        public string CouponCode { get; set; }

        public int UserId { get; set; }

        public int Sex { get; set; }

        public int? CouponID { get; set; }

        public string IP { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual Coupon Coupon { get; set; }

        public int? CouponItemID { get; set; }

        public int? CouponLogID { get; set; }

        public virtual CouponItem CouponItem { get; set; }

        public virtual CouponLog CouponLog { get; set; }
    }
}