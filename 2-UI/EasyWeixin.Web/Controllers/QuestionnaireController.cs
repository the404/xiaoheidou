using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Questionnaire")]
    public class QuestionnaireController : Controller
    {
        //调查问卷
        // GET: /Questionnaire/
        private readonly IUserProfileRepository UserProfileRepository;

        private readonly IQuestionCategoryRepository QuestionCategoryRepository;
        private readonly ISetQuestionRepository SetQuestionRepository;
        private readonly IQItemAnswerRepository QItemAnswerRepository;

        public QuestionnaireController(IQuestionCategoryRepository QuestionCategoryRepository,
            IUserProfileRepository UserProfileRepository,
            ISetQuestionRepository SetQuestionRepository,
            IQItemAnswerRepository QItemAnswerRepository)
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

        #region 主页列表

        public ActionResult QuestionIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var quest = QuestionCategoryRepository.FindAll(Specification<QuestionCategory>.Eval(o => o.UserId == UserId)).OrderByDescending(o => o.AddDate).ToList();
            var Pagerlist = QuestionCategoryRepository.GetListByPages(quest, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表

        #region 打开创建页

        public ActionResult QuestionCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult QuestionCreate(QuestionaireViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            form.Status = 1;
            form.StartDate = form.StartDate == null ? DateTime.Now : Convert.ToDateTime(form.StartDate);
            form.EndDate = form.EndDate == null ? DateTime.Now : Convert.ToDateTime(form.EndDate);
            if (ModelState.IsValid)
            {
                QuestionCategory pp = Mapper.Map<QuestionaireViewModel, QuestionCategory>(form);
                QuestionCategoryRepository.Add(pp);
                QuestionCategoryRepository.Context.Commit();
                switch (UserId.ToString())
                {
                    case "28":
                        //case "11":
                        //上海
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityQuestionaire/QuestionaireIndex?CatID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "27":
                        //天津
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityQuestionaire/TQuestionaireIndex?CatID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "29":
                        //武汉
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityQuestionaire/WQuestionaireIndex?CatID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "32":
                        //云南
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityQuestionaire/YQuestionaireIndex?CatID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "31":
                        //泰州
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityQuestionaire/ZQuestionaireIndex?CatID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    default:
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityQuestionaire/QuestionaireIndex?CatID=" + pp.ID + "&User_ID=" + user.ID;
                        break;
                }

                QuestionCategoryRepository.Update(pp);
                QuestionCategoryRepository.Context.Commit();
            }
            return Redirect("/Questionnaire/QuestionIndex");
        }

        #endregion 打开创建页

        #region 打开编辑页

        public ActionResult QuestionEdit(Guid id)
        {
            QuestionCategory pp = QuestionCategoryRepository.GetByKey(id);
            QuestionaireViewModel form = Mapper.Map<QuestionCategory, QuestionaireViewModel>(pp);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult QuestionEdit(QuestionaireViewModel form)
        {
            if (ModelState.IsValid)
            {
                QuestionCategory pp = QuestionCategoryRepository.GetByKey(form.ID);
                pp.CName = form.CName;
                pp.Content = form.Content;
                pp.StartDate = form.StartDate == null ? DateTime.Now : Convert.ToDateTime(form.StartDate);
                pp.EndDate = form.EndDate == null ? DateTime.Now : Convert.ToDateTime(form.EndDate);
                QuestionCategoryRepository.Update(pp);
                QuestionCategoryRepository.Context.Commit();
            }
            return Redirect("/Questionnaire/QuestionIndex");
        }

        public ActionResult QuestionDelete(Guid id)
        {
            var pp = QuestionCategoryRepository.GetByKey(id);
            if (pp != null)
            {
                var qitem = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.QuestionCategoryID == pp.CatID));
                foreach (var item in qitem)
                {
                    SetQuestionRepository.Remove(item);
                    SetQuestionRepository.Context.Commit();
                }
                QuestionCategoryRepository.Remove(pp);
                QuestionCategoryRepository.Context.Commit();
            }
            return Redirect("/Questionnaire/QuestionIndex");
        }

        public ActionResult OpenStatus(Guid PId)
        {
            QuestionCategory pp = QuestionCategoryRepository.GetByKey(PId);
            pp.Status = pp.Status == 1 ? 0 : 1;
            QuestionCategoryRepository.Update(pp);
            QuestionCategoryRepository.Context.Commit();
            return Redirect("QuestionIndex");
        }

        public ActionResult GetLink(Guid id)
        {
            try
            {
                QuestionCategory pp = QuestionCategoryRepository.GetByKey(id);
                return View(pp);
            }
            catch (Exception)
            {
                return Redirect("/Questionnaire/QuestionIndex");
            }
        }

        #endregion 打开编辑页

        #region 问题列表

        public ActionResult QuestionList(int qID, int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var quest = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.QuestionCategoryID == qID && o.UserId == UserId)).OrderBy(o => o.OrderIndex).ToList();
            var Pagerlist = SetQuestionRepository.GetListByPages(quest, pageid, 10);
            return View(Pagerlist);
        }

        public ActionResult QuestionItemCreate()
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var quest = QuestionCategoryRepository.FindAll(Specification<QuestionCategory>.Eval(o => o.UserId == UserId && o.Status == 1)).Select(o => new { o.CatID, o.CName }).ToList();
            ViewBag.Categories = new SelectList(quest, "CatID", "CName");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult QuestionItemCreate(SetQuestionViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            form.UserId = UserId;
            var ss = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(o => o.UserId == UserId && o.QuestionCategoryID == form.QuestionCategoryID)).ToList();

            if (ss.Count() > 0)
            {
                form.OrderIndex = ss.Max(qu => qu.SetQuestionID) + 1;
            }
            else
            {
                form.OrderIndex = 1;
            }

            SetQuestion pp = null;
            if (ModelState.IsValid)
            {
                pp = Mapper.Map<SetQuestionViewModel, SetQuestion>(form);
                SetQuestionRepository.Add(pp);
                SetQuestionRepository.Context.Commit();
            }
            return Redirect("/Questionnaire/QuestionList?qID=" + pp.QuestionCategoryID);
        }

        public ActionResult QuestionItemEdit(Guid id)
        {
            SetQuestion pp = SetQuestionRepository.GetByKey(id);
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var quest = QuestionCategoryRepository.FindAll(Specification<QuestionCategory>.Eval(o => o.UserId == UserId && o.Status == 1)).Select(o => new { o.CatID, o.CName }).ToList();
            ViewBag.Categories = new SelectList(quest, "CatID", "CName", pp.SetQuestionID.ToString());
            SetQuestionViewModel form = Mapper.Map<SetQuestion, SetQuestionViewModel>(pp);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult QuestionItemEdit(SetQuestionViewModel form)
        {
            SetQuestion pp = null;
            if (ModelState.IsValid)
            {
                pp = SetQuestionRepository.GetByKey(form.ID);
                pp.SetQuestionName = form.SetQuestionName;
                pp.IsOther = form.IsOther;
                pp.Status = form.Status;
                pp.Answers = form.Answers;
                pp.Type = form.Type;
                pp.QuestionCategoryID = form.QuestionCategoryID;
                SetQuestionRepository.Update(pp);
                SetQuestionRepository.Context.Commit();
            }
            return Redirect("/Questionnaire/QuestionList?qID=" + pp.QuestionCategoryID);
        }

        public ActionResult QuestionItemDelete(Guid id)
        {
            var pp = SetQuestionRepository.GetByKey(id);
            if (pp != null)
            {
                SetQuestionRepository.Remove(pp);
                SetQuestionRepository.Context.Commit();
            }
            return Redirect("/Questionnaire/QuestionList?qID=" + pp.QuestionCategoryID);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="PId"></param>
        /// <returns></returns>
        public ActionResult OrderIndexOperation(Guid PId, string act)
        {
            SetQuestion qq = SetQuestionRepository.GetByKey(PId);
            switch (act)
            {
                case "up":
                    SetQuestion top = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(obj => obj.Status == true && obj.OrderIndex < qq.OrderIndex && obj.QuestionCategoryID == qq.QuestionCategoryID)).OrderByDescending(obj => obj.OrderIndex).Take(1).SingleOrDefault();
                    if (top == null)
                    {
                        return Redirect("/Questionnaire/QuestionList?qID=" + qq.QuestionCategoryID);
                    }
                    int? currentIndex = qq.OrderIndex;
                    int? topIndex = top.OrderIndex;
                    qq.OrderIndex = topIndex;
                    top.OrderIndex = currentIndex;
                    SetQuestionRepository.Update(top);
                    SetQuestionRepository.Context.Commit();
                    break;

                case "down":
                    SetQuestion down = SetQuestionRepository.FindAll(Specification<SetQuestion>.Eval(obj => obj.Status == true && obj.OrderIndex > qq.OrderIndex && obj.QuestionCategoryID == qq.QuestionCategoryID)).OrderBy(obj => obj.OrderIndex).Take(1).SingleOrDefault();
                    if (down == null)
                    {
                        return Redirect("/Questionnaire/QuestionList?qID=" + qq.QuestionCategoryID);
                    }
                    int? currentIndex2 = qq.OrderIndex;
                    int? topIndex2 = down.OrderIndex;
                    qq.OrderIndex = topIndex2;
                    down.OrderIndex = currentIndex2;
                    SetQuestionRepository.Update(down);
                    SetQuestionRepository.Context.Commit();
                    break;

                default:
                    break;
            }
            SetQuestionRepository.Update(qq);
            SetQuestionRepository.Context.Commit();

            return Redirect("/Questionnaire/QuestionList?qID=" + qq.QuestionCategoryID);
        }

        /// <summary>
        /// 问卷调查结果
        /// </summary>
        /// <returns></returns>
        public ActionResult SetQuestionDrawing(Guid id, int Ischeck = 0, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            var setAns = SetQuestionRepository.GetByKey(id);
            var qi = QItemAnswerRepository.FindAll(Specification<QItemAnswer>.Eval(o => o.SetQuestionID == setAns.SetQuestionID));

            SurveyViewModel sm = new SurveyViewModel();
            sm.qName = setAns.SetQuestionName;
            sm.answerCount = qi.ToList().Count();
            sm.AType = Convert.ToInt32(setAns.Type);
            sm.SetQuestionID = id;
            List<QitemQus> qList = new List<QitemQus>();
            List<Suggest> slist = new List<Suggest>();

            //条件查询
            if (Ischeck == 0)
            {
                qi = QItemAnswerRepository.FindAll(Specification<QItemAnswer>.Eval(o => o.SetQuestionID == setAns.SetQuestionID));
            }
            else
            {
                if (dt1 != null && dt2 != null && dt1 <= dt2)
                {
                    qi = QItemAnswerRepository.FindAll(Specification<QItemAnswer>.Eval(o => o.SetQuestionID == setAns.SetQuestionID && o.AddDate >= dt1 && o.AddDate <= dt2));
                }
                else
                {
                    qi = QItemAnswerRepository.FindAll(Specification<QItemAnswer>.Eval(o => o.SetQuestionID == setAns.SetQuestionID));
                }
            }

            if (!String.IsNullOrEmpty(setAns.Answers))
            {
                //获得总数
                int sum = 0;
                var ArrName = setAns.Answers.Split(',').ToList();
                for (int i = 0; i < ArrName.Count; i++)
                {
                    sum += qi.Where(o => o.Answer.Contains("," + (i + 1).ToString() + ",")).ToList().Count;
                }
                if (setAns.IsOther == true)
                {
                    sum += qi.Where(o => o.Answer.Contains("," + "0" + ",")).ToList().Count;
                }

                //循环添加问题选项
                for (int i = 0; i < ArrName.Count; i++)
                {
                    QitemQus ss = new QitemQus();
                    int count = 0;
                    if (sum != 0)
                    {
                        count = qi.Where(o => o.Answer.Contains("," + (i + 1).ToString() + ",")).ToList().Count;
                        ss.dName = "答案" + (i + 1).ToString();
                        ss.iName = ArrName[i];
                        ss.per = (count * 1.0 / sum).ToString("P");
                    }
                    else
                    {
                        //建议进这里来了

                        ss.dName = "答案" + (i + 1).ToString();
                        ss.iName = ArrName[i];
                        ss.per = "0.00%";
                        //ss.dName = "建议" + (i + 1).ToString();
                        //ss.iName = qi.FirstOrDefault().Answer;
                        //ss.per = "0.00%";
                    }
                    qList.Add(ss);
                }

                //选其他的比例
                QitemQus sa = new QitemQus();
                if (setAns.IsOther == true)
                {
                    int count = qi.Where(o => o.Answer.Contains("," + "0" + ",")).ToList().Count;
                    sa.dName = "其他:";
                    sa.iName = "";
                    sa.per = (count * 1.0 / sum).ToString("P");
                    qList.Add(sa);
                }

                foreach (var mm in qi.ToList())
                {
                    Suggest sg = new Suggest();
                    int n = mm.Answer.IndexOf(",");
                    int m = mm.Answer.LastIndexOf(",");
                    sg.Suggestion = mm.Answer.Substring(n + 1, m - 1);
                    slist.Add(sg);
                }
            }
            sm.qlist = qList;
            sm.slist = slist;

            return View(sm);
        }

        #endregion 问题列表
    }
}