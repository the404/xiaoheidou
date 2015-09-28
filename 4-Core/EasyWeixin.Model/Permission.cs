using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    [Table("webpages_Permission")]
    public class Permission : IAggregateRoot
    {
        #region Ctor

        public Permission()
        {
            PermissionsInRoles = new List<PermissionsInRoles>();
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required, StringLength(200)]
        public string PermissionName { get; set; }

        [StringLength(500)]
        public string PermissionDesc { get; set; }

        [StringLength(200)]
        public string PermissionImageName { get; set; }

        public DateTime? AdminTime { get; set; }

        [StringLength(200)]
        public string PermissionActionName { get; set; }

        [StringLength(200)]
        public string PermissionUrl { get; set; }

        public int? IsOrder { get; set; }

        public int? IsMenu { get; set; }

        [StringLength(200)]
        public string PermissionChineseName { get; set; }

        public virtual ICollection<PermissionsInRoles> PermissionsInRoles { set; get; }
    }
}