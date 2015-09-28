using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class CouponLog : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CouponLogID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string CouponUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        public string CouponCode { get; set; }

        public int? CouponID { get; set; }

        public string IP { get; set; }

        public int IsAward { get; set; }

        public virtual Coupon Coupon { get; set; }
    }
}