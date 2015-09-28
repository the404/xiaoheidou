namespace EasyWeixin.Entities.Request.Events
{
    /// <summary>
    /// 事件之二维码扫描（关注微信）
    /// </summary>
    public class RequestMessageEvent_Scan : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.SCAN; }
        }

        public string Ticket { get; set; }
    }
}