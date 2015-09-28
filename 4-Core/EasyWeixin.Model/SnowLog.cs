using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class SnowLog : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SnowLogID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string SnowUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        /// <summary>
        /// 发送的数据
        /// </summary>
        public string SnowData { get; set; }

        /// <summary>
        /// 是否中奖，中间类型，0表示不中奖，1表示日场票，2表示夜场票，3表示纪念水杯，4表示报纸阅读卡
        /// </summary>
        public int IsAward { get; set; }

        public string IP { get; set; }

        public int? SnowID { get; set; }

        public virtual Snow Snow { get; set; }
    }
}