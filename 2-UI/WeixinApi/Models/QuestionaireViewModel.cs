using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Web.Models
{
    public class QuestionaireViewModel
    {
        public int CatID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "不得为空")]
        public string CName { get; set; }

        [Required(ErrorMessage = "不得为空")]
        public string Content { get; set; }

        public string GetURL { get; set; }

        public string GetShortURL { get; set; }

        public int? UserId { get; set; }

        public int? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? AddDate { get; set; }
    }
}