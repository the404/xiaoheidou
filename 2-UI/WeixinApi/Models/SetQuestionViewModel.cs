using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Web.Models
{
    public class SetQuestionViewModel
    {
        public int SetQuestionID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "不得为空")]
        public string SetQuestionName { get; set; }

        [Required(ErrorMessage = "不得为空")]
        public string Answers { get; set; }//问题选项

        public int? AnswerCount { get; set; }

        public int? Type { get; set; }

        public bool Status { get; set; }

        public int? OrderIndex { get; set; }

        public int? QuestionCategoryID { get; set; }

        public bool IsOther { get; set; }

        public int? UserId { get; set; }

        public DateTime? AddDate { get; set; }
    }
}