using System;
using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class ButtonViewModel
    {
        public int ButtonID { get; set; }

        public Guid ID { get; set; }

        [Required(ErrorMessage = "** 菜单名称不能为空"), StringLength(16, ErrorMessage = "** 按钮在16个字符的范围之内")]
        public string name { get; set; }

        public string type { get; set; }

        [StringLength(128)]
        public string key { get; set; }

        public int? ResponseMessageID { get; set; }

        public DateTime? AddTime { get; set; }

        public int IsOrder { get; set; }

        public int UserId { get; set; }

        public virtual ResponseMessageViewModel ResponseMessage { get; set; }
    }
}