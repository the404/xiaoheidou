using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class PayCustomer : IAggregateRoot
    {
        //支付成功用户表字段：手机号，支付信息，是否抽奖，支付时间，抽奖时间，奖品，openid
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int WID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string UPhone { get; set; }

        public string PayInfo { get; set; }

        public string OpendId { get; set; }

        public string Award { get; set; }

        public int? IsAward { get; set; }

        public DateTime? PayDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public string nickname { get; set; }

        public string headUrl { get; set; }
    }
}