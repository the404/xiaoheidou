using System;

namespace EasyWeixin.Web.Models
{
    public class CouponItemViewModel
    {
        public int CouponItemID { get; set; }

        public Guid ID { get; set; }

        /// <summary>
        /// 中奖项名称
        /// </summary>
        public string CouponItemName { get; set; }

        /// <summary>
        /// 设置中奖概率比例
        /// </summary>
        public int CouponItemScale { get; set; }

        public string CouponItemAward { get; set; }

        public int CouponID { get; set; }

        public string CouponCode { get; set; }

        public DateTime AddDate { get; set; }
    }
}