using System;

namespace EasyWeixin.Web.Models
{
    public class GuessNewsViewModel
    {
        public Guid ImageTextID { get; set; }

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

        public string Message { get; set; }

        public int CurrentAnswer { get; set; }

        public string IP { get; set; }

        public Guid User_ID { get; set; }

        public int isGuess { get; set; }

        public int? GuessID { get; set; }

        public string GuessDesc { get; set; }

        public string GuessTitle { get; set; }

        public string GuessStyle { get; set; }

        public string CompanyName { get; set; }
    }
}