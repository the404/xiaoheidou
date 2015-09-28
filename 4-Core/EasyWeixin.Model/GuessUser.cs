using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class GuessUser : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GuessUserID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string GuessUserName { get; set; }

        public string Identification { get; set; }

        public string GuessUserEmail { get; set; }

        public string GuessUserPhone { get; set; }

        public string GuessUserWexinID { get; set; }

        public DateTime AddDate { get; set; }

        public int GuessTimes { get; set; }

        public string GuessProcess { get; set; }

        public int Answer { get; set; }

        public int UserId { get; set; }

        public int Sex { get; set; }

        public int? GuessID { get; set; }

        public string IP { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual Guess Guess { get; set; }
    }
}