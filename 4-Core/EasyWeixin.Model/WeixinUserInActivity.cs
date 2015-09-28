using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    /// <summary>
    ///     表示微信用户在系统内每个活动的参与情况
    /// </summary>
    public class WeixinUserInActivity : IAggregateRoot
    {
        public int WeixinUserId { get; set; }

        /// <summary>
        ///     活动类型/大转盘,刮刮乐
        /// </summary>
        public ActType ActType { get; set; }

        /// <summary>
        ///     某项活动中具体活动的Id
        /// </summary>
        public int ActId { get; set; }

        /// <summary>
        ///     某些有限制参与次数的活动中表示剩余参与次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 一共参与次数的统计
        /// </summary>
        public int SumCount { get; set; }

        /// <summary>
        /// 记录每天分享后额外获得的抽奖次数
        /// </summary>
        public int TodayAdd { get; set; }

        /// <summary>
        /// 记录添加TodayAdd字段的时间，以此来判断每天只能添加几次，存一个DateTime.Now.Date.ToString()
        /// </summary>
        public string Today { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Column("WeixinUserId"), InverseProperty("WeixinUserInActivities")]
        public WeixinUser WeixinUser { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
    }

    public enum ActType
    {
        Guess = 1,
        Snow = 2,
        Scratch = 3,
        Wheel = 4,
        Vote = 5,
        Coupon = 6,
        Fight = 7,
        Fruit = 8,
        Ghost = 9,
        Prefer = 10,
        Activies = 11,
        Egg = 12
    }
}