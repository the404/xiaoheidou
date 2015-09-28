namespace EasyWeixin.Entities.Request.Events
{
    /// <summary>
    /// 事件之扫码推事件(scancode_push)
    /// </summary>
    public class RequestMessageEvent_Scancode_Push : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.scancode_push; }
        }

        /// <summary>
        /// 扫描信息
        /// </summary>
        public ScanCodeInfo ScanCodeInfo { get; set; }
    }
}