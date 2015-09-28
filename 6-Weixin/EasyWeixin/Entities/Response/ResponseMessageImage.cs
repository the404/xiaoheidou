using EasyWeixin;
using EasyWeixin.Entities.Response;

namespace Senparc.Weixin.MP.Entities
{
    public class ResponseMessageImage : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Image; }
        }

        public string PicUrl { get; set; }
    }
}