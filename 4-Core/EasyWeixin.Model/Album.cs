using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    /// <summary>
    /// 相册
    /// </summary>
    public class Album : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AlbumID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string AlbumName { get; set; }

        public string AlbumDesc { get; set; }

        public DateTime AddDate { get; set; }

        public int? UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<AlbumItem> AlbumItems { get; set; }
    }
}