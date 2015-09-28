namespace EasyWeixin.Entities.Request
{
    public class RequestMessageImage : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Image; }
        }

        public string PicUrl { get; set; }
    }
}