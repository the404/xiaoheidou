namespace EasyWeixin.Entities.Response
{
    public class ResponseMessageVideo : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Video; }
        }

        public string VideoUrl { get; set; }
    }
}