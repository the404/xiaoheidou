using System;

namespace EasyWeixin.Web.Models
{
    public class WheelUserViewModel
    {
        public int WheelUserID { get; set; }

        public Guid ID { get; set; }

        public string WheelUserName { get; set; }

        public string Identification { get; set; }

        public string WheelUserEmail { get; set; }

        public string WheelUserPhone { get; set; }

        public string WheelUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        /// <summary>
        /// 获奖者真实姓名
        /// </summary>
        public string WheelTrueName { get; set; }

        /// <summary>
        /// 获奖等级
        /// </summary>
        public string WheelRank { get; set; }

        /// <summary>
        /// 获取的大转盘的编号
        /// </summary>
        public string WheelCode { get; set; }

        /// <summary>
        /// 获取的大转盘的角度
        /// </summary>
        public string WheelAngle { get; set; }

        public int UserId { get; set; }

        public int Sex { get; set; }

        public int? WheelID { get; set; }

        public string IP { get; set; }
    }
}