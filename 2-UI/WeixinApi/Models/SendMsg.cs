namespace EasyWeixin.Web.Models
{
    public class SendMsg
    {
        public string touser { get; set; }

        public string msgtype { get; set; }

        public TextMsg text { get; set; }
    }

    public class TextMsg
    {
        public string content { get; set; }
    }
}