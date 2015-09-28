using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class Vote : IAggregateRoot
    {
        private string getUrl;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int VoteID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string VoteTitle { get; set; }

        public string VoteDesc { get; set; }

        public string VoteAnswer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string GetURL
        {
            get { return getUrl.ReplaceHost(); }
            set { getUrl = value.ReplaceHost(); }
        }

        public string GetShortURL { get; set; }

        public string QuickResponse { get; set; }

        public string VoteStyle { get; set; }

        public int VoteType { get; set; }

        public int VoteIsOther { get; set; }

        public int? UserId { get; set; }

        public int? EveryDayTimes { get; set; }

        public int ResponseImageTextID { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime AddDate { get; set; }

        public virtual ResponseImageText ResponseImageText { get; set; }

        public virtual ICollection<VoteUser> VoteUsers { get; set; }
    }
}