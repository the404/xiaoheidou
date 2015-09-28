namespace EasyWeixin.Entities.JsonResult
{
    public class JsApiTicketResult
    {
        public string errorcode { get; set; }

        public string errormsg { get; set; }

        public string ticket { get; set; }

        public string vsdshFKA { get; set; }

        public int expires_in { get; set; }
    }
}