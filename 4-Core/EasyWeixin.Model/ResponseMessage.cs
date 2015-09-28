using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class ResponseMessage : IAggregateRoot
    {
        #region Ctor

        public ResponseMessage()
            : base()
        {
            Content = string.Empty;
            Link = string.Empty;
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResponseMessageID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int? UserId { get; set; }

        /// <summary>
        /// 消息类型0 添加到按键 1被添加自动回复 2消息自动回复 3关键字回复
        /// </summary>
        public int? ResponseType { get; set; }

        /// <summary>
        /// 按钮类型可能跳转链接 可能做图文回复 0文字 1语音 2图片 3视频 4 图文消息 5链接
        /// </summary>
        public int? ButtonType { get; set; }

        /// <summary>
        /// 文字
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 语音
        /// </summary>
        public int? ResponseMusicID { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public int? ResponseImageID { get; set; }

        /// <summary>
        /// 图文
        /// </summary>
        public int? ResponseImageTextID { get; set; }

        /// <summary>
        /// 视频
        /// </summary>
        public int? ResponseVideoID { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        public DateTime? AddTime { get; set; }

        public int? ResponseKeyRuleID { get; set; }

        public int? ButtonID { get; set; }

        public int? SubButtonID { get; set; }

        public virtual Button Button { get; set; }

        public virtual SubButton SubButton { get; set; }

        public virtual ResponseMusic ResponseMusic { get; set; }

        public virtual ResponseImage ResponseImage { get; set; }

        public virtual ResponseImageText ResponseImageText { get; set; }

        public virtual ResponseVideo ResponseVideo { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ResponseKeyRule ResponseKeyRule { get; set; }
    }
}