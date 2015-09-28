namespace EasyWeixin.Entities.Menu
{
    /// <summary>
    /// 单个按键
    /// </summary>
    public class SingleButton : BaseButton, IBaseButton
    {
        /// <summary>
        /// 按钮类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 类型为click时必须。
        /// 按钮KEY值，用于消息接口(event类型)推送，不超过128字节
        /// </summary>
      //  public string key { get; set; }

        ///// <summary>
        ///// 类型为view时必须
        ///// 网页链接，用户点击按钮可打开链接，不超过256字节
        ///// </summary>
        //public string url { get; set; }

        public SingleButton(string theType)
        {
            type = theType;
        }
    }
}