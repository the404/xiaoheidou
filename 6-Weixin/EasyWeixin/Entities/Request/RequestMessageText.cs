using EasyWeixin;
using EasyWeixin.Entities.Request;

namespace Senparc.Weixin.MP.Entities
{
    public class RequestMessageText : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Text; }
        }

        public string Content { get; set; }
    }
}