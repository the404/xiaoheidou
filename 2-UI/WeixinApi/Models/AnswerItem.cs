using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Web.Models
{
    public class AllAnswer
    {
        public int SetQuestionID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string SetQuestionName { get; set; }

        public string Answers { get; set; }//问题选项

        public int? Type { get; set; }

        public int? QuestionCategoryID { get; set; }

        public bool IsOther { get; set; }

        public int? UserId { get; set; }

        public List<AnswerItem> aList = new List<AnswerItem>();
    }

    public class AnswerItem
    {
        public string QuestionId { get; set; }

        public string AnswerName { get; set; }
    }
}