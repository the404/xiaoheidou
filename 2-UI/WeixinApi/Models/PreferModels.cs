using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Web.Models
{
    public class PreferModels
    {
        public int PreID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "优惠标题不得为空")]
        public string Name { get; set; }

        public string Explanation { get; set; }

        [Required(ErrorMessage = "优惠内容不得为空")]
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

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CreateIp { get; set; }

        public DateTime? AddDate { get; set; }
    }
}