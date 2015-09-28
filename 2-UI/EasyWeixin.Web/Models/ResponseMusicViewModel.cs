using System;

namespace EasyWeixin.Web.Models
{
    public class ResponseMusicViewModel
    {
        public int ResponseMusicID { get; set; }

        public Guid ID { get; set; }

        public string MusicUrl { get; set; }

        public string HQMusicUrl { get; set; }

        public string MusicName { get; set; }

        public DateTime? AddTime { get; set; }

        public int UserId { get; set; }
    }
}