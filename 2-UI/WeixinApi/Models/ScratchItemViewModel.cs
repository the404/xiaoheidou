using System;

namespace EasyWeixin.Web.Models
{
    public class ScratchItemViewModel
    {
        public int ScratchItemID { get; set; }

        public Guid ID { get; set; }

        /// <summary>
        /// 中奖项名称
        /// </summary>
        public string ScratchItemName { get; set; }

        /// <summary>
        /// 设置中奖概率比例
        /// </summary>
        public int ScratchItemScale { get; set; }

        public string ScratchItemAward { get; set; }

        /// <summary>
        /// 展示顺序
        /// </summary>
        public int isOrder { get; set; }

        public string ImageUrl { get; set; }

        public int ScratchID { get; set; }

        public string ScratchCode { get; set; }

        public DateTime AddDate { get; set; }
    }
}