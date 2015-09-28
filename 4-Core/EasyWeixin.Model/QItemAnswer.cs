using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class QItemAnswer : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int QItemAnswerID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int? SetQuestionID { get; set; }

        public int? UserId { get; set; }

        public string Answer { get; set; }//问题选项

        public string OtherAnswer { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime? AddDate { get; set; }
    }
}