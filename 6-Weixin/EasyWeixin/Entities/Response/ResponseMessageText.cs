using EasyWeixin;
using EasyWeixin.Entities.Response;

namespace Senparc.Weixin.MP.Entities
{
    public class ResponseMessageText : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Text; }
        }

        public string Content { get; set; }
    }
}