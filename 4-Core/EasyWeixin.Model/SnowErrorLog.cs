using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class SnowErrorLog : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SnowErrorLogID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Action { get; set; }

        public string Message { get; set; }

        public DateTime AddDate { get; set; }

        public string IP { get; set; }

        public int? SnowID { get; set; }

        public virtual Snow Snow { get; set; }
    }
}