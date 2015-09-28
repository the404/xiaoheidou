using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ResponseImageText : IAggregateRoot
    {
        private string url, picUrl;

        #region Ctor

        public ResponseImageText()
            : base()
        {
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResponseImageTextID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(200)]
        public string ImageTextName { get; set; }

        public string PicUrl
        {
            get { return picUrl.ReplaceHost(); }
            set { picUrl = value.ReplaceHost(); }
        }

        public string Url
        {
            get { return url.ReplaceHost(); }
            set { url = value.ReplaceHost(); }
        }

        public DateTime? AddTime { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 图文类型 1 代表活动 101代表活动中的魔法猜猜猜
        /// </summary>
        public int ImageTextType { get; set; }

        public string ImageTextDesc { get; set; }

        public virtual ICollection<Guess> Guesses { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<ResponseMessage> ResponseMessages { get; set; }
    }
}