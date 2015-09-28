using System;

namespace EasyWeixin.Web.Models
{
    public class ResponseImageViewModel
    {
        public int ResponseImageID { get; set; }

        public Guid ID { get; set; }

        public string ImageUrl { get; set; }

        public string ImageName { get; set; }

        public DateTime? AddTime { get; set; }

        public int UserId { get; set; }
    }
}