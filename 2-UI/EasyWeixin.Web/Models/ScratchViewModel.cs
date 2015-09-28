using CustomAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class ScratchViewModel
    {
        public int ScratchID { get; set; }

        public Guid ID { get; set; }

        [Required(ErrorMessage = "活动标题不得为空")]
        public string ScratchTitle { get; set; }

        [Required(ErrorMessage = "活动说明不得为空")]
        public string ScratchDesc { get; set; }

        [DataType(DataType.Time, ErrorMessage = "请输入正确的时间")]
        [Required(ErrorMessage = "开始时间不得为空")]
        [DateTimeCompare("EndDate", ValueComparison.IsLessThanOrEqual, ErrorMessage = "开始时间必须小于或等于结束时间")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Time, ErrorMessage = "请输入正确的时间")]
        [Required(ErrorMessage = "结束时间不得为空")]
        [DateTimeCompare("StartDate", ValueComparison.IsGreaterThanOrEqual, ErrorMessage = "结束时间必须大于或等于开始时间")]
        public DateTime EndDate { get; set; }

        public string GetURL { get; set; }

        public string GetShortURL { get; set; }

        public string QuickResponse { get; set; }

        public string ScratchStyle { get; set; }

        public string ScratchBgImage { get; set; }

        public int ScratchScale { get; set; }

        public int? UserId { get; set; }

        public string Thanks { get; set; }

        public int? EveryDayTimes { get; set; }

        public int ResponseImageTextID { get; set; }

        //   public virtual UserProfile UserProfile { get; set; }
        public DateTime AddDate { get; set; }

        public virtual ResponseImageTextViewModel ResponseImageTextViewModel { get; set; }

        public virtual ICollection<ScratchUserViewModel> ScratchUsers { get; set; }

        public virtual ICollection<ScratchUserViewModel> MyAwards { get; set; }

        public string PicUrl { get; set; }
    }
}