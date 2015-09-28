namespace EasyWeixin.Entities.Request.Events
{
    /// <summary>
    /// 自定义菜单点击跳转链接事件
    /// </summary>
    public class RequestMessageEvent_View : RequestMessageEventBase, IRequestMessageEventBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.VIEW; }
        }
    }
}