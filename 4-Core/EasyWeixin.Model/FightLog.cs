using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class FightLog : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FightLogID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string FightUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        public string FightLogAnswer { get; set; }

        public int AnswerResult { get; set; }

        public int QuestionIndex { get; set; }

        public int? FightItemID { get; set; }

        public string IP { get; set; }

        public virtual FightItem FightItem { get; set; }
    }
}