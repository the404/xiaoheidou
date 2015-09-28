using System;

namespace EasyWeixin.Web.Models
{
    public class EggItemViewModel
    {
        public int WheelItemID { get; set; }

        public Guid ID { get; set; }

        /// <summary>
        /// 中奖项名称
        /// </summary>
        public string WheelItemName { get; set; }

        /// <summary>
        /// 设置中奖概率比例
        /// </summary>
        public int WheelItemScale { get; set; }

        public string WheelItemAward { get; set; }

        public string WheelCode { get; set; }

        public string nickname { get; set; }

        public string headUrl { get; set; }

        public int WheelID { get; set; }

        public DateTime AddDate { get; set; }
    }
}