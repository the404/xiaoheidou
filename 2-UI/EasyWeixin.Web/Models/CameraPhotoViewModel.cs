using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Web.Models
{
    public class CameraPhotoViewModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CameraID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string YName { get; set; }

        public string Remark { get; set; }

        public DateTime? AddTime { get; set; }

        public string IpAddress { get; set; }

        public int? State { get; set; }

        public int? LoveNum { get; set; }

        public int? IsZan { get; set; }

        public int? PhotoID { get; set; }
    }
}