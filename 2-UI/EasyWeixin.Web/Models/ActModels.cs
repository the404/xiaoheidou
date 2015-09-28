using CustomAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace EasyWeixin.Web.Models
{
    public class ActModels
    {
        public int ActID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "活动标题不得为空")]
        public string Name { get; set; }

        [AllowHtml]
        public string Explanation { get; set; }

        [AllowHtml]

        [Required(ErrorMessage = "活动内容不得为空")]
        public string Content { get; set; }

        public string GetURL { get; set; }

        public string GetShortURL { get; set; }

        public int? UserId { get; set; }

        public int? IsTop { get; set; }

        public DateTime? TopTime { get; set; }

        public int? Clicks { get; set; }

        public string ImageUrl { get; set; }

        public string WURL { get; set; } //微信公众号链接

        public string ClubName { get; set; } //欢乐谷名称

        [DataType(DataType.DateTime, ErrorMessage = "请选择有效时间")]
        //[Range(typeof(DateTime), "1/1/1990", "1/1/2200")]
        [Required]
        [DateTimeCompare("EndDate", ValueComparison.IsLessThanOrEqual, ErrorMessage = "开始时间必须小于或等于结束时间")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "请选择有效时间")]
        //[Range(typeof(DateTime), "1/1/1990", "1/1/2200")]
        [Required]
        [DateTimeCompare("StartDate", ValueComparison.IsGreaterThanOrEqual, ErrorMessage = "结束时间必须大于或等于开始时间")]

        public DateTime? EndDate { get; set; }

        public string CreateIp { get; set; }

        public DateTime? AddDate { get; set; }
    }
}