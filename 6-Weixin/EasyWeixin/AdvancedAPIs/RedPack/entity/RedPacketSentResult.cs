using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.AdvancedAPIs.RedPack
{
    public class RedPacketSentResult
    {
        public bool Succeeded { get; set; }
        /// <summary>
        /// 微信服务器回复的xml
        /// </summary>
        public string Response { get; set; }

        public RedPacketSentError Error { get; set; }
        /// <summary>
        /// 总金额,并不是从服务器获取的
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 订单号,并不是从服务器获取的
        /// </summary>
        public string BillNumber { get; set; }

    }
}
