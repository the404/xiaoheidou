using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class CameraPhoto : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CameraID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Name { get; set; } //缩约图路径

        public string YName { get; set; }

        public string Remark { get; set; } //原图路径

        public DateTime? AddTime { get; set; }

        public string IpAddress { get; set; }

        public int? State { get; set; }

        public int? LoveNum { get; set; }

        public bool IsCheck { get; set; }

        public int? PhotoID { get; set; }
    }
}