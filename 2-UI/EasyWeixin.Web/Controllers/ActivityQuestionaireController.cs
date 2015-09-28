using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityQuestionaireController : Controller
    {
        //调查问卷
        // GET: /ActivityQuestionaire/

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IQuestionCategoryRepository QuestionCategoryRepository;
        private readonly ISetQuestionRepository SetQuestionRepository;
        private readonly IQItemAnswerRepository QItemAnswerRepository;

        public ActivityQuestionaireController(IQuestionCategoryRepository QuestionCategoryRepository,
            ISetQuestionRepository SetQuestionRepository,
            IQItemAnswerRepository QItemAnswerRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.UserProfileRepository = UserProfileRepository;
            this.QuestionCategoryRepository = QuestionCategoryRepository;
            this.SetQuestionRepository = SetQuestionRepository;
            this.QItemAnswerRepository = QItemAnswerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 上海

        public ActionResult QuestionaireIndex(Guid CatID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            var quest = QuestionCategoryRepository.Find(Specification<QuestionCategory>.Eval(o => o.UserId == user.UserId && o.ID == CatID && o.Status == 1));
            List<AllAnswer> aalist = new List<AllAnswer>();
            if (quest != null)
            {
                var qr = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.UserId == user.UserId && o.QuestionCategoryID == quest.CatID && o.Status == true)).OrderBy(o => o.OrderIndex).ToList();

                foreach (var item in qr)
                {
                    AllAnswer aa = new AllAnswer();
                    aa.SetQuestionName = item.SetQuestionName;
                    aa.Type = item.Type;
                    aa.QuestionCategoryID = item.QuestionCategoryID;
                    aa.ID = item.ID;
                    aa.SetQuestionID = item.SetQuestionID;
                    aa.IsOther = Convert.ToBoolean(item.IsOther);
                    aa.Answers = item.Answers;
                    aa.UserId = user.UserId;
                    List<AnswerItem> silist = new List<AnswerItem>();
                    if (item.Type == 0)  //单选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int i = 0; i < answers.Count; i++)
                            {
                                AnswerItem mm = new AnswerItem();
                                mm.AnswerName = answers[i];
                                mm.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(mm);
                            }
                            //if (item.IsOther == true)
                            //{
                            //    AnswerItem mq = new AnswerItem();
                            //    mq.QuestionId = item.SetQuestionID.ToString();
                            //    mq.AnswerName = "其他";
                            //    silist.Add(mq);
                            //}
                        }
                    }
                    if (item.Type == 1)  //多选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int j = 0; j < answers.Count; j++)
                            {
                                AnswerItem ad = new AnswerItem();
                                ad.AnswerName = answers[j];
                                ad.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(ad);
                            }

                            //if (item.IsOther == true)
                            //{
                            //    AnswerItem aq = new AnswerItem();
                            //    aq.AnswerName = "其他";
                            //    aq.QuestionId = item.SetQuestionID.ToString();
                            //    silist.Add(aq);
                            //}
                        }
                    }
                    //if(item.Type==-1)
                    //{
                    //    if (!String.IsNullOrEmpty(item.Answers))
                    //    {
                    //        AnswerItem nn = new AnswerItem();
                    //        nn.AnswerName = item.Answers;
                    //        nn.QuestionId = item.SetQuestionID.ToString();
                    //        silist.Add(nn);
                    //    }
                    //}
                    aa.aList = silist;
                    aalist.Add(aa);
                }
            }
            return View(aalist);
        }

        #endregion 上海

        #region 天津

        public ActionResult TQuestionaireIndex(Guid CatID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            var quest = QuestionCategoryRepository.Find(Specification<QuestionCategory>.Eval(o => o.UserId == user.UserId && o.ID == CatID && o.Status == 1));
            List<AllAnswer> aalist = new List<AllAnswer>();
            if (quest != null)
            {
                var qr = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.UserId == user.UserId && o.QuestionCategoryID == quest.CatID && o.Status == true)).OrderBy(o => o.OrderIndex).ToList();

                foreach (var item in qr)
                {
                    AllAnswer aa = new AllAnswer();
                    aa.SetQuestionName = item.SetQuestionName;
                    aa.Type = item.Type;
                    aa.QuestionCategoryID = item.QuestionCategoryID;
                    aa.ID = item.ID;
                    aa.SetQuestionID = item.SetQuestionID;
                    aa.IsOther = Convert.ToBoolean(item.IsOther);
                    aa.Answers = item.Answers;
                    aa.UserId = user.UserId;
                    List<AnswerItem> silist = new List<AnswerItem>();
                    if (item.Type == 0)  //单选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int i = 0; i < answers.Count; i++)
                            {
                                AnswerItem mm = new AnswerItem();
                                mm.AnswerName = answers[i];
                                mm.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(mm);
                            }
                        }
                    }
                    if (item.Type == 1)  //多选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int j = 0; j < answers.Count; j++)
                            {
                                AnswerItem ad = new AnswerItem();
                                ad.AnswerName = answers[j];
                                ad.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(ad);
                            }
                        }
                    }
                    aa.aList = silist;
                    aalist.Add(aa);
                }
            }
            return View(aalist);
        }

        #endregion 天津

        #region 武汉

        public ActionResult WQuestionaireIndex(Guid CatID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            var quest = QuestionCategoryRepository.Find(Specification<QuestionCategory>.Eval(o => o.UserId == user.UserId && o.ID == CatID && o.Status == 1));
            List<AllAnswer> aalist = new List<AllAnswer>();
            if (quest != null)
            {
                var qr = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.UserId == user.UserId && o.QuestionCategoryID == quest.CatID && o.Status == true)).OrderBy(o => o.OrderIndex).ToList();

                foreach (var item in qr)
                {
                    AllAnswer aa = new AllAnswer();
                    aa.SetQuestionName = item.SetQuestionName;
                    aa.Type = item.Type;
                    aa.QuestionCategoryID = item.QuestionCategoryID;
                    aa.ID = item.ID;
                    aa.SetQuestionID = item.SetQuestionID;
                    aa.IsOther = Convert.ToBoolean(item.IsOther);
                    aa.Answers = item.Answers;
                    aa.UserId = user.UserId;
                    List<AnswerItem> silist = new List<AnswerItem>();
                    if (item.Type == 0)  //单选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int i = 0; i < answers.Count; i++)
                            {
                                AnswerItem mm = new AnswerItem();
                                mm.AnswerName = answers[i];
                                mm.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(mm);
                            }
                        }
                    }
                    if (item.Type == 1)  //多选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int j = 0; j < answers.Count; j++)
                            {
                                AnswerItem ad = new AnswerItem();
                                ad.AnswerName = answers[j];
                                ad.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(ad);
                            }
                        }
                    }
                    aa.aList = silist;
                    aalist.Add(aa);
                }
            }
            return View(aalist);
        }

        #endregion 武汉

        #region 云南

        public ActionResult YQuestionaireIndex(Guid CatID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            List<AllAnswer> aalist = new List<AllAnswer>();
            var quest = QuestionCategoryRepository.Find(Specification<QuestionCategory>.Eval(o => o.UserId == user.UserId && o.ID == CatID && o.Status == 1));
            if (quest != null)
            {
                var qr = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.UserId == user.UserId && o.QuestionCategoryID == quest.CatID && o.Status == true)).OrderBy(o => o.OrderIndex).ToList();

                foreach (var item in qr)
                {
                    AllAnswer aa = new AllAnswer();
                    aa.SetQuestionName = item.SetQuestionName;
                    aa.Type = item.Type;
                    aa.QuestionCategoryID = item.QuestionCategoryID;
                    aa.ID = item.ID;
                    aa.SetQuestionID = item.SetQuestionID;
                    aa.IsOther = Convert.ToBoolean(item.IsOther);
                    aa.Answers = item.Answers;
                    aa.UserId = user.UserId;
                    List<AnswerItem> silist = new List<AnswerItem>();
                    if (item.Type == 0)  //单选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int i = 0; i < answers.Count; i++)
                            {
                                AnswerItem mm = new AnswerItem();
                                mm.AnswerName = answers[i];
                                mm.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(mm);
                            }
                        }
                    }
                    if (item.Type == 1)  //多选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int j = 0; j < answers.Count; j++)
                            {
                                AnswerItem ad = new AnswerItem();
                                ad.AnswerName = answers[j];
                                ad.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(ad);
                            }
                        }
                    }
                    aa.aList = silist;
                    aalist.Add(aa);
                }
            }
            return View(aalist);
        }

        #endregion 云南

        #region 泰州

        public ActionResult ZQuestionaireIndex(Guid CatID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            var quest = QuestionCategoryRepository.Find(Specification<QuestionCategory>.Eval(o => o.UserId == user.UserId && o.ID == CatID && o.Status == 1));
            List<AllAnswer> aalist = new List<AllAnswer>();
            if (quest != null)
            {
                var qr = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.UserId == user.UserId && o.QuestionCategoryID == quest.CatID && o.Status == true)).OrderBy(o => o.OrderIndex).ToList();

                foreach (var item in qr)
                {
                    AllAnswer aa = new AllAnswer();
                    aa.SetQuestionName = item.SetQuestionName;
                    aa.Type = item.Type;
                    aa.QuestionCategoryID = item.QuestionCategoryID;
                    aa.ID = item.ID;
                    aa.SetQuestionID = item.SetQuestionID;
                    aa.IsOther = Convert.ToBoolean(item.IsOther);
                    aa.Answers = item.Answers;
                    aa.UserId = user.UserId;
                    List<AnswerItem> silist = new List<AnswerItem>();
                    if (item.Type == 0)  //单选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int i = 0; i < answers.Count; i++)
                            {
                                AnswerItem mm = new AnswerItem();
                                mm.AnswerName = answers[i];
                                mm.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(mm);
                            }
                        }
                    }
                    if (item.Type == 1)  //多选
                    {
                        if (!String.IsNullOrEmpty(item.Answers))
                        {
                            var answers = item.Answers.Split(',').ToList();
                            for (int j = 0; j < answers.Count; j++)
                            {
                                AnswerItem ad = new AnswerItem();
                                ad.AnswerName = answers[j];
                                ad.QuestionId = item.SetQuestionID.ToString();
                                silist.Add(ad);
                            }
                        }
                    }
                    aa.aList = silist;
                    aalist.Add(aa);
                }
            }
            return View(aalist);
        }

        #endregion 泰州

        public JsonResult AddQuestion(string questionData, string answerData, string User_Id)
        {
            var flag = false;
            //|4, |1,3,4, |dfdsf,
            //处理重复值
            questionData = questionData.TrimEnd(',');
            string[] qq = questionData.Split(',');
            string[] aa = answerData.Split('|');
            List<string> allList = new List<string>(qq);
            List<string> qllList = new List<string>(aa);
            allList = allList.Distinct().ToList();
            qllList = qllList.ToList();
            if (allList.Count > 0)
            {
                for (int i = 0; i < allList.Count; i++)
                {
                    //插入提交答案
                    QItemAnswer qitemanswer = new QItemAnswer();
                    //for (int j = 0; j < qllList.Count; j++)
                    //{
                    qitemanswer.SetQuestionID = Convert.ToInt32(allList[i]);
                    qitemanswer.Answer = "," + qllList[i + 1];
                    qitemanswer.AddDate = DateTime.Now;
                    qitemanswer.UserId = Convert.ToInt32(User_Id);
                    //break;
                    //}
                    QItemAnswerRepository.Add(qitemanswer);
                    QItemAnswerRepository.Context.Commit();
                    flag = true; ;
                }
            }

            return Json(new { flag = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}