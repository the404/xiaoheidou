using CustomAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class VoteViewModel
    {
        public int VoteID { get; set; }

        public Guid ID { get; set; }

        [Required(ErrorMessage = "活动标题不得为空")]
        public string VoteTitle { get; set; }

        [Required(ErrorMessage = "活动说明不得为空")]
        public string VoteDesc { get; set; }

        [Required(ErrorMessage = "活动答案不得为空")]
        public string VoteAnswer { get; set; }

        [Required(ErrorMessage = "开始时间不得为空")]
        [DateTimeCompare("EndDate", ValueComparison.IsLessThanOrEqual, ErrorMessage = "开始时间必须小于或等于结束时间")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "结束时间不得为空")]
        [DateTimeCompare("StartDate", ValueComparison.IsGreaterThanOrEqual, ErrorMessage = "结束时间必须大于或等于开始时间")]
        public DateTime EndDate { get; set; }

        public string GetURL { get; set; }

        public string GetShortURL { get; set; }

        public string QuickResponse { get; set; }

        public string VoteStyle { get; set; }

        public int VoteType { get; set; }

        public int VoteIsOther { get; set; }

        public int? UserId { get; set; }

        public int? EveryDayTimes { get; set; }

        public int ResponseImageTextID { get; set; }

        public DateTime AddDate { get; set; }

        public virtual ResponseImageTextViewModel ResponseImageTextViewModel { get; set; }
    }
}