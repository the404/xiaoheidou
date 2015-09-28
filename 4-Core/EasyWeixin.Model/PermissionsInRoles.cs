using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    [Table("webpages_PermissionsInRoles")]
    public class PermissionsInRoles : IAggregateRoot
    {
        #region Ctor

        public PermissionsInRoles()
            : base()
        {
        }

        #endregion Ctor

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        [Column("RoleId"), InverseProperty("PermissionsInRoles")]
        public Role Role { get; set; }

        [Column("PermissionId"), InverseProperty("PermissionsInRoles")]
        public Permission Permission { get; set; }
    }
}