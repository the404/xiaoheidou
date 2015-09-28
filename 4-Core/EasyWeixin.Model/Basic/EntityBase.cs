using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.Model.Basic
{
    public class EntityBase<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }

        [Required]
        public bool Enabled { get; set; }
        [Required]
        public int CreateUserId { get; set; }
        [Required]
        public int UpdateUserId { get; set; }
        public DateTime createTime { get; set; }


    }
}
