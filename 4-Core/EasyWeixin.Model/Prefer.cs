using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class Prefer : IAggregateRoot
    {
        private string getUrl;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PreID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Explanation { get; set; }

        public string Content { get; set; }

        public string GetURL
        {
            get { return getUrl.ReplaceHost(); }
            set { getUrl = value.ReplaceHost(); }
        }

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

        public virtual UserProfile UserProfile { get; set; }

        public DateTime? AddDate { get; set; }
    }
}