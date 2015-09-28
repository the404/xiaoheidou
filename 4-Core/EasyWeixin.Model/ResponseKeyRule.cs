using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ResponseKeyRule : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResponseKeyRuleID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required, StringLength(200)]
        public string RuleName { get; set; }

        public int? UserId { get; set; }

        public DateTime? AddTime { get; set; }

        public int? IsOrder { get; set; }

        public virtual ICollection<ResponseKey> ResponseKeys { get; set; }

        public virtual ICollection<ResponseMessage> ResponseMessages { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}