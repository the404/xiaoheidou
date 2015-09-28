using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ScratchUser : IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ScratchUserID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
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

        /// <summary>
        /// 是否领奖
        /// </summary>
        public bool IsAward { get; set; }

        public virtual Scratch Scratch { get; set; }

        public virtual ScratchItem ScratchItem { get; set; }

        public virtual WeixinUser WeixinUser { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        /// <summary>
        /// 领奖日期
        /// </summary>
        public string AwardDate { get; set; }
    }
}