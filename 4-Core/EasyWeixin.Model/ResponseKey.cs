using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ResponseKey : IAggregateRoot
    {
        #region Ctor

        public ResponseKey()
            : base()
        {
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResponseKeyID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required, StringLength(50)]
        public string Key { get; set; }

        public int? UserId { get; set; }

        public int? IsFullMatch { get; set; }

        public DateTime? AddTime { get; set; }

        public int? IsOrder { get; set; }

        public int ResponseKeyRuleID { get; set; }

        public virtual ResponseKeyRule ResponseKeyRules { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}