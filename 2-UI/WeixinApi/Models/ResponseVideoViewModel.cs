using System;

namespace EasyWeixin.Web.Models
{
    public class ResponseVideoViewModel
    {
        public int ResponseVideoID { get; set; }

        public Guid ID { get; set; }

        public string VideoUrl { get; set; }

        public string VideoName { get; set; }

        public DateTime? AddTime { get; set; }

        public int UserId { get; set; }
    }
}