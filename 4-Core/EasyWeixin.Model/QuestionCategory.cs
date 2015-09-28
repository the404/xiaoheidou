using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class QuestionCategory : IAggregateRoot
    {
        private string getUrl;

        //CatID,ID,CName, Content, Status,StartDate, EndDate,CreateDate
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CatID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string CName { get; set; }

        public string Content { get; set; }

        public int? Status { get; set; }

        public string GetURL
        {
            get { return getUrl.ReplaceHost(); }
            set { getUrl = value.ReplaceHost(); }
        }

        public string GetShortURL { get; set; }

        public int? UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime? AddDate { get; set; }
    }
}