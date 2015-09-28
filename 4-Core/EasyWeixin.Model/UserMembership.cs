using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    [Table("webpages_Membership")]
    public class UserMembership : IAggregateRoot
    {
        #region Ctor

        public UserMembership()
            : base()
        {
            Roles = new List<Role>();
        }

        #endregion Ctor

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(128)]
        public string ConfirmationToken { get; set; }

        public bool? IsConfirmed { get; set; }

        public DateTime? LastPasswordFailureDate { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        [Required, StringLength(128)]
        public string Password { get; set; }

        public DateTime? PasswordChangedDate { get; set; }

        [Required, StringLength(128)]
        public string PasswordSalt { get; set; }

        [StringLength(128)]
        public string PasswordVerificationToken { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        [StringLength(128)]
        public string PasswordResetToken { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}