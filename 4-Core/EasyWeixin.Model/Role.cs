using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    [Table("webpages_Roles")]
    public class Role : IAggregateRoot
    {
        #region Ctor

        public Role()
        {
            UserMemberships = new List<UserMembership>();
            PermissionsInRoles = new List<PermissionsInRoles>();
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required, StringLength(256)]
        public string RoleName { get; set; }

        [StringLength(300)]
        public string RoleDesc { get; set; }

        [StringLength(300)]
        public string RoleImageName { get; set; }

        [StringLength(300)]
        public string RoleControllerName { get; set; }

        public DateTime? AddTime { get; set; }

        public int? IsOrder { get; set; }

        public int? IsMenu { get; set; }

        [StringLength(256)]
        public string RoleChineseName { get; set; }

        public virtual ICollection<UserMembership> UserMemberships { get; set; }

        public virtual ICollection<PermissionsInRoles> PermissionsInRoles { set; get; }
    }
}