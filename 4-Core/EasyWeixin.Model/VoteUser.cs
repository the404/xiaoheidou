using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class VoteUser : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int VoteUserID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string VoteUserName { get; set; }

        public string Identification { get; set; }

        public string VoteUserEmail { get; set; }

        public string VoteUserPhone { get; set; }

        public string VoteUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        public string VoteTrueName { get; set; }

        public int VoteIndex { get; set; }

        public int UserId { get; set; }

        public int Sex { get; set; }

        public int? VoteID { get; set; }

        public string IP { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual Vote Vote { get; set; }
    }
}