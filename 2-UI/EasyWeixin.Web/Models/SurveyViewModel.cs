using System;
using System.Collections.Generic;

namespace EasyWeixin.Web.Models
{
    public class SurveyViewModel
    {
        public Guid SetQuestionID { get; set; }

        public string qName { get; set; }

        public int answerCount { get; set; }

        public int AType { get; set; }

        public List<QitemQus> qlist = new List<QitemQus>();
        public List<Suggest> slist = new List<Suggest>();
    }

    public class QitemQus
    {
        public string dName { get; set; } //例如，答案1

        public string iName { get; set; } //问题名称

        public string per { get; set; }//百分比
    }

    public class Suggest
    {
        public string Suggestion { get; set; }
    }
}