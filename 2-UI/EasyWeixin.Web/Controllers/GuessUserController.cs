using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Guess")]
    public class GuessUserController : Controller
    {
        //魔法猜猜猜
        // GET: /GuessUser/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IGuessUserRepository GuessUserRepository;
        private readonly IGuessRepository GuessRepository;

        public GuessUserController(IGuessRepository GuessRepository, IGuessUserRepository GuessUserRepository, IResponseImageTextRepository ResponseImageTextRepository, IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.GuessUserRepository = GuessUserRepository;
            this.GuessRepository = GuessRepository;
        }
        
        public ActionResult GuessCreate()
        {
            return View();
        }

        public ActionResult GuessIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Guess = GuessRepository.FindAll(Specification<Guess>.Eval(o => o.UserId == UserId)).ToList();
            var Pagerlist = GuessRepository.GetListByPages(Guess, pageid, 10);
            return View(Pagerlist);
        }

        public ActionResult GuessEdit(Guid id)
        {
            Guess guess = GuessRepository.GetByKey(id);
            GuessViewModel form = Mapper.Map<Guess, GuessViewModel>(guess);
            form.ResponseImageTextViewModel = Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(guess.ResponseImageText);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GuessEdit(GuessViewModel form)
        {
            if (ModelState.IsValid)
            {
                Guess guess = GuessRepository.GetByKey(form.ID);
                guess.GuessTitle = form.GuessTitle;
                guess.StartDate = form.StartDate;
                guess.EndDate = form.EndDate;
                guess.GuessDesc = form.GuessDesc;
                guess.ResponseImageText.ImageTextName = form.GuessTitle;
                guess.ResponseImageText.Content = form.ResponseImageTextViewModel.Content;
                GuessRepository.Update(guess);
                GuessRepository.Context.Commit();
            }
            return View(form);
        }

        public ActionResult GuessDelete(Guid id)
        {
            var guess = GuessRepository.GetByKey(id);
            if (guess.GuessUsers.ToList().Count < 500)
            {
                foreach (var item in guess.GuessUsers.ToList())
                {
                    GuessUserRepository.Remove(item);
                    GuessUserRepository.Context.Commit();
                }
                GuessRepository.Remove(guess);
                GuessRepository.Context.Commit();
            }
            return RedirectToAction("GuessIndex");
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GuessCreate(GuessViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.ResponseImageTextViewModel.ImageTextName = form.GuessTitle;
            form.ResponseImageTextViewModel.ImageTextType = 101;
            form.ResponseImageTextViewModel.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.ResponseImageTextViewModel.AddTime = DateTime.Now;

            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                Guess guess = Mapper.Map<GuessViewModel, Guess>(form);
                guess.GuessStyle = "GuessNews.css";
                guess.ResponseImageText = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);
                GuessRepository.Add(guess);
                GuessRepository.Context.Commit();
                guess.ResponseImageText.Url = "http://" + Request.Url.Host + "/News/GuessNews?ImageTextID=" + guess.ResponseImageText.ID;
                guess.GetURL = "http://" + Request.Url.Host + "/News/GuessNews?ImageTextID=" + guess.ResponseImageText.ID + "&User_ID=" + user.ID;
                GuessRepository.Update(guess);
                GuessRepository.Context.Commit();
            }
            return View(form);
        }

        public ActionResult GetLink(Guid id)
        {
            Guess guess = GuessRepository.GetByKey(id);
            return View(guess);
        }

        public ActionResult ResponseMessageCreateOrEdit()
        {
            var model = ResponseImageTextRepository.Find(Specification<ResponseImageText>.Eval(o => o.ImageTextType == 101 && o.UserId == WebSecurity.GetUserId(User.Identity.Name)));
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResponseMessageCreateOrEdit(ResponseImageText ResponseImageText)
        {
            ResponseImageText.UserId = WebSecurity.GetUserId(User.Identity.Name);
            ResponseImageText.AddTime = DateTime.Now;
            if (ResponseImageText.ResponseImageTextID == 0)
            {
                ResponseImageText.ImageTextType = 101;
                ResponseImageTextRepository.Add(ResponseImageText);
                ResponseImageTextRepository.Context.Commit();
                ResponseImageText.Url = "http://" + Request.Url.Host + "/News/GuessNews?ImageTextID=" + ResponseImageText.ID;
                ResponseImageTextRepository.Update(ResponseImageText);
                ResponseImageTextRepository.Context.Commit();
                return View(ResponseImageText);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ResponseImageText.Url = "http://" + Request.Url.Host + "/News/GuessNews?ImageTextID=" + ResponseImageText.ID;
                    ResponseImageTextRepository.Update(ResponseImageText);
                    ResponseImageTextRepository.Context.Commit();
                    return View(ResponseImageText);
                }
            }
            return View(ResponseImageText);
        }

        public ActionResult GuessUserList(Guid GuessID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            int guesscount = 0;
            int guesspeoplecount = 0;
            if (dt1 != null && dt2 != null && dt1 <= dt2)
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Guess g = GuessRepository.GetByKey(GuessID);
                dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                var list = GuessUserRepository.FindAll(Specification<GuessUser>.Eval(o => o.UserId == UserId && o.GuessID == g.GuessID && o.AddDate > dt1 && o.AddDate < dt2)).OrderBy(o => o.GuessTimes).ToList();
                guesscount = list.Count;
                guesspeoplecount = list.GroupBy(o => o.GuessUserPhone).ToList().Count;
                var Pagerlist = GuessUserRepository.GetListByPages(list, pageid, 10);
                ViewData["guesscount"] = guesscount;
                ViewData["guesspeoplecount"] = guesspeoplecount;
                return View(Pagerlist);
            }
            else
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Guess g = GuessRepository.GetByKey(GuessID);
                var EndDate = g.EndDate.AddDays(1);
                var list = GuessUserRepository.FindAll(Specification<GuessUser>.Eval(o => o.UserId == UserId && o.GuessID == g.GuessID && o.AddDate > g.StartDate && o.AddDate < EndDate)).OrderBy(o => o.GuessTimes).ToList();
                guesscount = list.Count;
                guesspeoplecount = list.GroupBy(o => o.GuessUserPhone).ToList().Count;
                ViewData["guesscount"] = guesscount;
                ViewData["guesspeoplecount"] = guesspeoplecount;
                var Pagerlist = GuessUserRepository.GetListByPages(list, pageid, 10);
                return View(Pagerlist);
            }
        }

        public ActionResult Delete(Guid id)
        {
            GuessUser permission = GuessUserRepository.GetByKey(id);
            GuessUserRepository.Remove(permission);
            GuessUserRepository.Context.Commit();
            return RedirectToAction("GuessUserList");
        }

        public JsonResult DeleteAll(string IDlist)
        {
            var list = IDlist.Split('|');
            for (int i = 0; i < list.Length; i++)
            {
                GuessUser permission = GuessUserRepository.GetByKey(Guid.Parse(list[i]));
                GuessUserRepository.Remove(permission);
                GuessUserRepository.Context.Commit();
            }
            return Json(new { Message = "删除成功" });
        }

        public string GetQrCodeImage(string qrEncoding)
        {
            string txt_qr = "Byte";
            string Level = "M";
            string txt_ver = "7";
            string txt_size = "4";
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            String encoding = qrEncoding;
            if (encoding == "Byte")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            else if (encoding == "AlphaNumeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            else if (encoding == "Numeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }
            try
            {
                int scale = Convert.ToInt16(txt_size);
                qrCodeEncoder.QRCodeScale = scale;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                int version = Convert.ToInt16(txt_ver);
                qrCodeEncoder.QRCodeVersion = version;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            string errorCorrect = Level;
            if (errorCorrect == "L")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            else if (errorCorrect == "M")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            else if (errorCorrect == "Q")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            else if (errorCorrect == "H")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

            Image image;
            String data = txt_qr;
            image = qrCodeEncoder.Encode(data);
            string filename = Guid.NewGuid().ToString() + ".jpg";
            string filepath = Server.MapPath(@"~\images\QRCode\upload") + "\\" + filename;
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
            return filepath;
        }

        /// <summary>
        /// 10进制或16进制转换为中文
        /// </summary>
        /// <param name="name">要转换的字符串</param>
        /// <param name="fromBase">进制（10或16）</param>
        /// <returns></returns>
        public string ConverToGB(string text, int fromBase)
        {
            string value = text;
            MatchCollection mc;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            switch (fromBase)
            {
                case 10:

                    MatchCollection mc1 = Regex.Matches(text, @"&#([\d]{5})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    foreach (Match _v in mc1)
                    {
                        string w = _v.Value.Substring(2);
                        w = Convert.ToString(int.Parse(w), 16);
                        byte[] c = new byte[2];
                        string ss = w.Substring(0, 2);
                        int c1 = Convert.ToInt32(w.Substring(0, 2), 16);
                        int c2 = Convert.ToInt32(w.Substring(2), 16);
                        c[0] = (byte)c2;
                        c[1] = (byte)c1;
                        sb.Append(Encoding.Unicode.GetString(c));
                    }
                    value = sb.ToString();

                    break;

                case 16:
                    mc = Regex.Matches(text, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (mc != null && mc.Count > 0)
                    {
                        foreach (Match m2 in mc)
                        {
                            string v = m2.Value;
                            string w = v.Substring(2);
                            byte[] c = new byte[2];
                            int c1 = Convert.ToInt32(w.Substring(0, 2), 16);
                            int c2 = Convert.ToInt32(w.Substring(2), 16);
                            c[0] = (byte)c2;
                            c[1] = (byte)c1;
                            sb.Append(Encoding.Unicode.GetString(c));
                        }
                        value = sb.ToString();
                    }
                    break;
            }
            return value;
        }
    }
}