using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class Guess : IAggregateRoot
    {
        private string getUrl;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GuessID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string GuessTitle { get; set; }

        public string GuessName { get; set; }

        public string GuessDesc { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string GetURL
        {
            get { return getUrl.ReplaceHost(); }
            set { getUrl = value.ReplaceHost(); }
        }

        public string GetShortURL { get; set; }

        public string QuickResponse { get; set; }

        public string GuessStyle { get; set; }

        public int? UserId { get; set; }

        public int ResponseImageTextID { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime AddDate { get; set; }

        public virtual ResponseImageText ResponseImageText { get; set; }

        public virtual ICollection<GuessUser> GuessUsers { get; set; }
    }
}