using EasyWeixin.Model;
using System;

namespace EasyWeixin.Web.Models
{
    public class WeixinUserInActivitiesViewModel
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

        public WeixinUserViewModel WeixinUser { get; set; }

        public Guid ID { get; set; }
    }
}