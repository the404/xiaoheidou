using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class FightUserItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FightUserItemID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string FightUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        public string FightUserAnswer { get; set; }

        public int AnswerResult { get; set; }

        public int QuestionIndex { get; set; }

        public int? FightItemID { get; set; }

        public int? FightUserID { get; set; }

        public string IP { get; set; }

        public virtual FightItem FightItem { get; set; }

        public virtual FightUser FightUser { get; set; }
    }
}