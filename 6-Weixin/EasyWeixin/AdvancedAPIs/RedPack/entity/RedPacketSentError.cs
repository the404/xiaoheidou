using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.AdvancedAPIs.RedPack
{
    public enum RedPacketSentError : byte
    {
        None = 0,
        InternalError = 1,
        BalanceNotEnough = 2, //余额不足
        Other = 128
    }
}
