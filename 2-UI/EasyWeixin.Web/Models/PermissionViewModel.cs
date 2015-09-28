using System;
using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class PermissionViewModel
    {
        public int PermissionId { get; set; }

        public Guid ID { get; set; }

        [Required(ErrorMessage = "权限路径为必填"), StringLength(200, ErrorMessage = "权限路径不超过200字符")]
        public string PermissionName { get; set; }

        [StringLength(500, ErrorMessage = "权限描述不超过500字符")]
        public string PermissionDesc { get; set; }

        [StringLength(200, ErrorMessage = "权限图片名不超过200字符")]
        public string PermissionImageName { get; set; }

        public DateTime? AdminTime { get; set; }

        [StringLength(200, ErrorMessage = "权限控制器不超过200字符")]
        public string PermissionActionName { get; set; }

        [StringLength(200, ErrorMessage = "权限路径不超过200字符")]
        public string PermissionUrl { get; set; }

        public int? IsOrder { get; set; }

        public int? IsMenu { get; set; }

        [StringLength(200, ErrorMessage = "权限名称不超过200字符")]
        public string PermissionChineseName { get; set; }
    }
}