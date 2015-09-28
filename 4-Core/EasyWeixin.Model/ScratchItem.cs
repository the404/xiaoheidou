using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ScratchItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ScratchItemID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        /// <summary>
        /// 中奖项名称
        /// </summary>
        public string ScratchItemName { get; set; }

        /// <summary>
        /// 设置中奖概率比例
        /// </summary>
        public int ScratchItemScale { get; set; }

        private string _scratchItemAward;

        public string ScratchItemAward
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_scratchItemAward))
                    return ScratchItemName;
                return _scratchItemAward;
            }
            set
            {
                _scratchItemAward = value;
            }
        }

        /// <summary>
        /// 展示顺序
        /// </summary>
        public int isOrder { get; set; }

        public string ImageUrl { get; set; }

        public int ScratchID { get; set; }

        public DateTime AddDate { get; set; }

        public virtual Scratch Scratch { get; set; }
    }
}