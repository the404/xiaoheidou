using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class FightItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FightItemID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string FightItemName { get; set; }

        public string FightItemAnswers { get; set; }

        public string FightItemTrueAnswer { get; set; }

        public int FightID { get; set; }

        public DateTime AddDate { get; set; }

        public virtual Fight Fight { get; set; }

        public virtual ICollection<FightUserItem> FightUserItems { get; set; }
    }
}