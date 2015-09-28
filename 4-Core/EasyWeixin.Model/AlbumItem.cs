using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class AlbumItem : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AlbumItemID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string AlbumItemTitle { get; set; }

        public string AlbumItemDesc { get; set; }

        public string AlbumItemPicSrc { get; set; }

        public DateTime AddDate { get; set; }

        public int AlbumID { get; set; }

        public virtual Album Album { get; set; }
    }
}