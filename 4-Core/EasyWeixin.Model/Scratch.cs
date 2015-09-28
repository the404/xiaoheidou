using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class Scratch : IAggregateRoot
    {
        private string getUrl;
        private string picUrl;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScratchID { get; set; }

        public string ScratchTitle { get; set; }

        public string ScratchDesc { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string GetURL
        {
            get { return getUrl.ReplaceHost(); }
            set { getUrl = value.ReplaceHost(); }
        }

        public string GetShortURL { get; set; }

        public string QuickResponse { get; set; }

        public string ScratchStyle { get; set; }

        public string ScratchBgImage { get; set; }

        public int ScratchScale { get; set; }

        public int? UserId { get; set; }

        public string Thanks { get; set; }

        public int? EveryDayTimes { get; set; }

        public int ResponseImageTextID { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime AddDate { get; set; }

        public virtual ResponseImageText ResponseImageText { get; set; }

        public virtual ICollection<ScratchUser> ScratchUsers { get; set; }

        public virtual ICollection<ScratchItem> ScratchItems { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string PicUrl
        {
            get { return picUrl.ReplaceHost(); }
            set { picUrl = value.ReplaceHost(); }
        }
    }
}