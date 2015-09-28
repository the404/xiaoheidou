using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ResponseMusic : IAggregateRoot
    {
        #region Ctor

        public ResponseMusic()
            : base()
        {
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResponseMusicID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [StringLength(400)]
        public string MusicUrl { get; set; }

        [StringLength(400)]
        public string HQMusicUrl { get; set; }

        [StringLength(200)]
        public string MusicName { get; set; }

        public string Description { get; set; }

        public DateTime? AddTime { get; set; }

        public int UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<ResponseMessage> ResponseMessages { get; set; }
    }
}