using Apworks;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class WheelLog : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int WheelLogID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string WheelUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        /// <summary>
        /// 获取的大转盘编号
        /// </summary>
        public string WheelCode { get; set; }

        /// <summary>
        /// 获取的大转盘的角度
        /// </summary>
        public string WheelAngle { get; set; }

        public int? WheelID { get; set; }

        public string IP { get; set; }

        public int IsAward { get; set; }

        public int IsShare { get; set; }

        public virtual Wheel Wheel { get; set; }
    }
}