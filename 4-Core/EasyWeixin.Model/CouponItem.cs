using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class CouponItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CouponItemID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string CouponItemName { get; set; }

        /// <summary>
        /// 设置中奖概率比例
        /// </summary>
        public int CouponItemScale { get; set; }

        public string CouponItemAward { get; set; }

        public int CouponID { get; set; }

        public DateTime AddDate { get; set; }

        public virtual Coupon Coupon { get; set; }
    }
}