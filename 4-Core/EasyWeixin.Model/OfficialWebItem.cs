using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class OfficialWebItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OfficialWebID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string OfficialWebTitle { get; set; }

        public string OfficialWebDesc { get; set; }

        public string OfficialWebBgPic { get; set; }

        public DateTime AddDate { get; set; }

        public int? UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<OfficialWebItem> OfficialWebItems { get; set; }
    }
}