using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class SetQuestion : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SetQuestionID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int? QuestionCategoryID { get; set; }

        public string SetQuestionName { get; set; }

        public string Answers { get; set; }//问题选项

        public int? AnswerCount { get; set; }//问题选项个数

        public int? Type { get; set; }

        public bool? Status { get; set; }

        public int? OrderIndex { get; set; }

        public bool? IsOther { get; set; }

        public int? UserId { get; set; }

        public DateTime? AddDate { get; set; }
    }
}