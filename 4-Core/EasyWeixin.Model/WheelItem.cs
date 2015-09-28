using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class WheelItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int WheelItemID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
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

        /// <summary>
        /// 展示顺序
        /// </summary>
        public int isOrder { get; set; }

        /// <summary>
        /// 最小角度，可以是数组字符串
        /// </summary>
        public string MinAngle { get; set; }

        /// <summary>
        /// 最大角度，可以是数组字符串
        /// </summary>
        public string MaxAngle { get; set; }

        public int WheelID { get; set; }

        public DateTime AddDate { get; set; }

        public virtual Wheel Wheel { get; set; }
    }
}