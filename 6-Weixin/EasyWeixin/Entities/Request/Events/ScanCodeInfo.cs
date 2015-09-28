namespace EasyWeixin.Entities.Request.Events
{
    /// <summary>
    /// 扫码事件中的ScanCodeInfo
    /// </summary>
    public class ScanCodeInfo
    {
        public string ScanType { get; set; }

        public string ScanResult { get; set; }
    }
}