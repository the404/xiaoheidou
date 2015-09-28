namespace EasyWeixin.Entities.Request.Events
{
    public class RequestMessageEvent_Enter : RequestMessageEventBase, IRequestMessageEventBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.ENTER; }
        }
    }
}