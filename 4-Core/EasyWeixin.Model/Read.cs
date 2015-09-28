using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    /// <summary>
    /// 阅读数表
    /// </summary>
    public class Read : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReadID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string ViewUrl { get; set; }

        public string CreateIp { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}