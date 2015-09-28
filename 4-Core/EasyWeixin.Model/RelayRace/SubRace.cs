using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model.RelayRace
{
    /// <summary>
    /// 接力赛帮忙跑的小伙伴
    /// </summary>
    public class SubRace : IAggregateRoot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubRaceId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int? WeixinUserId { get; set; }

        public int MainRaceId { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        public double Score { get; set; }
        public DateTime AddTime { get; set; }
        

        public WeixinUser WeixinUser { get; set; }

        public MainRace MainRace { get; set; }

        public SubRace()
        {
            AddTime = DateTime.Now;
        }
    }
}
