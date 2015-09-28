using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class PhotoWall : IAggregateRoot
    {
        private string getUrl;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PhotoID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string PhotoTitle { get; set; }

        public string PhotoDesc { get; set; }

        public string GetURL
        {
            get { return getUrl.ReplaceHost(); }
            set { getUrl = value.ReplaceHost(); }
        }

        public string GetShortURL { get; set; }

        public int? UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime? AddDate { get; set; }

        public virtual ICollection<CameraPhoto> CameraPhoto { get; set; }
    }
}