namespace EasyWeixin.Entities.Request.Events
{
    /// <summary>
    /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
    /// </summary>
    public class RequestMessageEvent_Scancode_Waitmsg : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.scancode_waitmsg; }
        }

        /// <summary>
        /// 扫描信息
        /// </summary>
        public ScanCodeInfo ScanCodeInfo { get; set; }
    }
}