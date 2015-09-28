using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class FightUser : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FightUserID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string FightUserName { get; set; }

        public string Identification { get; set; }

        public string FightUserEmail { get; set; }

        public string FightUserPhone { get; set; }

        public string FightUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        public string FightTrueName { get; set; }

        public string FightItemSum { get; set; }

        public int UserId { get; set; }

        public int Sex { get; set; }

        public int? FightID { get; set; }

        public string IP { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual Fight Fight { get; set; }

        public virtual ICollection<FightUserItem> FightUserItems { get; set; }
    }
}