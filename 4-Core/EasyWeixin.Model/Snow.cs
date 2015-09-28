using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class Snow : IAggregateRoot
    {
        private string getUrl;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SnowID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string SnowTitle { get; set; }

        public string SnowDesc { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string GetURL
        {
            get { return getUrl.ReplaceHost(); }
            set { getUrl = value.ReplaceHost(); }
        }

        public string GetShortURL { get; set; }

        public string QuickResponse { get; set; }

        public string SnowStyle { get; set; }

        public string SnowBgImage { get; set; }

        public int SnowScale { get; set; }

        public int? UserId { get; set; }

        public string Thanks { get; set; }

        public int? EveryDayTimes { get; set; }

        public int ResponseImageTextID { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime AddDate { get; set; }

        public int Mark { get; set; }

        public virtual ResponseImageText ResponseImageText { get; set; }

        public virtual ICollection<SnowUser> SnowUsers { get; set; }

        public virtual ICollection<SnowItem> SnowItems { get; set; }

        public virtual ICollection<SnowErrorLog> SnowErrorLogs { get; set; }
    }
}