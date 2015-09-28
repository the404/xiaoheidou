using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.AdvancedAPIs.RedPack
{
    public class RedPacket
    {
        /// <summary>
        /// 订单号格式： 商户号 + 4位年 + 2位月 + 2位日 + 10位自然日内唯一数字。
        /// </summary>
        public string BillNumber { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string SendName { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActName { get; set; }
        /// <summary>
        /// 祝福语
        /// </summary>
        public string Wishing { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public string AppId { get; set; }
        /// <summary>
        /// 要发送的用户的openId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 触发操作的客户的IP地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public int Amount { get; set; }

        public bool IsValid() //可以用ModelBinding机制来替换这个。
        {
            return !string.IsNullOrWhiteSpace(BillNumber) &&
                !string.IsNullOrWhiteSpace(SendName) &&
                !string.IsNullOrWhiteSpace(ActName) &&
                !string.IsNullOrWhiteSpace(Wishing) &&
                !string.IsNullOrWhiteSpace(Remark) &&
                !string.IsNullOrWhiteSpace(AppId) &&
                !string.IsNullOrWhiteSpace(OpenId) &&
                !string.IsNullOrWhiteSpace(IpAddress) &&
                ((Amount == 0) || (Amount != 0 && Amount >= 100));
        }
    }
}
