using System.Collections.Generic;

namespace EasyWeixin.Web.Models
{
    public class SearchResult
    {
        public string status { get; set; }

        public int count { get; set; }

        public int count_total { get; set; }

        public int pages { get; set; }

        public List<Post> posts { get; set; }
    }

    //public class Post
    //{
    //    public int id { get; set; }
    //    public string type { get; set; }
    //    public string url { get; set; }
    //    public string title { get; set; }
    //    public string content { get; set; }
    //    public string thumbnail { get; set; }

    //}
}