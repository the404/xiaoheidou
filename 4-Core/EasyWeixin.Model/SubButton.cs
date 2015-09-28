using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class SubButton : IAggregateRoot
    {
        #region 构造函数

        public SubButton()
        {
        }

        #endregion 构造函数

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SubButtonID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [MaxLength(16)]
        public string name { get; set; }

        public string type { get; set; }

        [MaxLength(128)]
        public string key { get; set; }

        public DateTime? AddTime { get; set; }

        public int IsOrder { get; set; }

        //public int? ResponseMessageID { get; set; }
        //public virtual ResponseMessage ResponseMessage { get; set; }

        public virtual ICollection<ResponseMessage> ResponseMessages { get; set; }

        public int ButtonID { get; set; }

        public virtual Button Button { get; set; }
    }
}