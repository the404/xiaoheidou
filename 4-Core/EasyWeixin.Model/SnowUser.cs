using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class SnowUser : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SnowUserID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string SnowUserName { get; set; }

        public string Identification { get; set; }

        public string SnowUserEmail { get; set; }

        public string SnowUserPhone { get; set; }

        public string SnowUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        public string SnowTrueName { get; set; }

        /// <summary>
        /// 过程记录
        /// </summary>
        public string SnowProgress { get; set; }

        /// <summary>
        /// 中途中奖类型
        /// </summary>
        public int IsAward { get; set; }

        public int? UserId { get; set; }

        public int Sex { get; set; }

        public int? SnowID { get; set; }

        public string IP { get; set; }

        public int Score { get; set; }

        public int? SnowLogID { get; set; }

        public virtual SnowLog SnowLog { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual Snow Snow { get; set; }
    }
}