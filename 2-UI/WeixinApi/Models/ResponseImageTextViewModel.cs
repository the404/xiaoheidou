using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyWeixin.Web.Models
{
    public class ResponseImageTextViewModel
    {
        public int ResponseImageTextID { get; set; }

        public Guid ID { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "图文内容不得为空！")]
        public string Content { get; set; }

        //取消这个检查是因为在其他活动中有一个创建图文内容的东西
        // 但是没有绑定这个属性，所以若干添加检查会导致其他活动无法成功添加
        //所以将这个检查的操作放到添加图文自己的页面中去
        //[Required(ErrorMessage = "标题不能为空!")]
        public string ImageTextName { get; set; }

        public DateTime? AddTime { get; set; }

        public int UserId { get; set; }

        public string PicUrl { get; set; }

        public string Url { get; set; }

        public int ImageTextType { get; set; }

        public string ImageTextDesc { get; set; }
    }
}