using EasyWeixin;
using EasyWeixin.Entities.Response;

namespace Senparc.Weixin.MP.Entities
{
    public class ResponseMessageMusic : ResponseMessageBase, IResponseMessageBase
    {
        public override ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Music; }
        }

        public Music Music { get; set; }

        public ResponseMessageMusic()
        {
            Music = new Music();
        }
    }
}