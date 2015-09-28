using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class SnowItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SnowItemID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string SnowItemName { get; set; }

        public int SnowScore { get; set; }

        public int SnowItemScale { get; set; }

        /// <summary>
        /// 分值类型，0高分值，1代表低分值
        /// </summary>
        public int SnowScoreType { get; set; }

        public int SnowID { get; set; }

        public DateTime AddDate { get; set; }

        public virtual Snow Snow { get; set; }
    }
}