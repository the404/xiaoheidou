using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model.RelayRace
{
    /// <summary>
    /// 接力赛的发起人
    /// </summary>
    public class MainRace : IAggregateRoot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MainRaceId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int? WeixinUserId { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 总成绩
        /// </summary>
        public double SumScore { get; set; }

        public DateTime AddTime { get; set; }

        public WeixinUser WeixinUser { get; set; }

        public virtual ICollection<SubRace> SubRaces { get; set; }
        public MainRace()
        {
            AddTime = DateTime.Now;
        }
    }
}
