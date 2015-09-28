using CustomAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Web.Models
{
    public class SnowViewModel
    {
        public int SnowID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "活动标题不得为空")]
        public string SnowTitle { get; set; }

        [Required(ErrorMessage = "活动说明不得为空")]
        public string SnowDesc { get; set; }

        [Required(ErrorMessage = "开始时间不得为空")]
        [DateTimeCompare("EndDate", ValueComparison.IsLessThanOrEqual, ErrorMessage = "开始时间必须小于或等于结束时间")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "结束时间不得为空")]
        [DateTimeCompare("StartDate", ValueComparison.IsGreaterThanOrEqual, ErrorMessage = "结束时间必须大于或等于开始时间")]
        public DateTime EndDate { get; set; }

        public string GetURL { get; set; }

        public string GetShortURL { get; set; }

        public string QuickResponse { get; set; }

        public string SnowStyle { get; set; }

        public string SnowBgImage { get; set; }

        public int SnowScale { get; set; }

        public int? UserId { get; set; }

        public string Thanks { get; set; }

        public int? EveryDayTimes { get; set; }

        public int ResponseImageTextID { get; set; }

        public virtual ResponseImageTextViewModel ResponseImageTextViewModel { get; set; }

        public DateTime AddDate { get; set; }
    }
}