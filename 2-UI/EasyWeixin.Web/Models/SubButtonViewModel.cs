using System;
using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class SubButtonViewModel
    {
        public int SubButtonID { get; set; }

        public Guid ID { get; set; }

        [Required(ErrorMessage = "** 菜单名称不能为空"), StringLength(40, ErrorMessage = "** 按钮在40个字符的范围之内")]
        public string name { get; set; }

        public string type { get; set; }

        public string key { get; set; }

        public DateTime? AddTime { get; set; }

        public int IsOrder { get; set; }

        public int? ResponseMessageID { get; set; }

        public int ButtonID { get; set; }

        public virtual ResponseMessageViewModel ResponseMessage { get; set; }
    }
}