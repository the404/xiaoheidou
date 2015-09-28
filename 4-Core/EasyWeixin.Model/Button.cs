using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class Button : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ButtonID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [StringLength(16)]
        public string name { get; set; }

        public string type { get; set; }

        [StringLength(128)]
        public string key { get; set; }

        //  public int? ResponseMessageID { get; set; }
        public virtual ICollection<ResponseMessage> ResponseMessages { get; set; }

        public DateTime? AddTime { get; set; }

        public int IsOrder { get; set; }

        public int UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<SubButton> SubButtons { get; set; }
    }
}