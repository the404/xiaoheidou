using System;

namespace EasyWeixin.Web.Models
{
    public class WeixinUserInUsersViewModel
    {
        public int UserId { get; set; }

        public int WeixinUserId { get; set; }

        public UserProfile UserProfile { get; set; }

        public WeixinUserViewModel WeixinUser { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Guid ID { get; set; }
    }
}