using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ResponseImage : IAggregateRoot
    {
        #region Ctor

        public ResponseImage()
            : base()
        {
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResponseImageID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required, StringLength(400)]
        public string ImageUrl { get; set; }

        [StringLength(200)]
        public string ImageName { get; set; }

        public string Description { get; set; }

        public DateTime? AddTime { get; set; }

        public int UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<ResponseMessage> ResponseMessages { get; set; }
    }
}