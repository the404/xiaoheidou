using System;

namespace EasyWeixin.Web.Models
{
    public class ScratchUserViewModel
    {
        public int ScratchUserID { get; set; }

        public Guid ID { get; set; }

        public DateTime AddDate { get; set; }

        /// <summary>
        /// 获取的刮刮卡编号
        /// </summary>
        public string ScratchCode { get; set; }

        public string IP { get; set; }

        public int? ScratchItemID { get; set; }

        public int? WeixinUserId { get; set; }

        public int? ScratchID { get; set; }

        public bool IsAward { get; set; }

        public virtual ScratchViewModel Scratch { get; set; }

        public virtual ScratchItemViewModel ScratchItem { get; set; }

        public virtual WeixinUserViewModel WeixinUser { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string AwardDate { get; set; }
    }
}