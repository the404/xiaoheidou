using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Web.Models
{
    public class CameraViewModel
    {
        public int PhotoID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "标题不得为空")]
        public string PhotoTitle { get; set; }

        [Required(ErrorMessage = "说明不得为空")]
        public string PhotoDesc { get; set; }

        public string GetURL { get; set; }

        public string GetShortURL { get; set; }

        public int? UserId { get; set; }

        public DateTime AddDate { get; set; }
    }
}