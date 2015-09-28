using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    /// <summary>
    /// 点赞数表
    /// </summary>
    public class Praise : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string ViewUrl { get; set; }

        public string CreateIp { get; set; }

        public string Cookie { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}