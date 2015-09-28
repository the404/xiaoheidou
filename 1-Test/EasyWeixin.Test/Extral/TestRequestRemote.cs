using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.HttpUtility;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace EasyWeixin.Test.Extral
{
    [TestClass]
    public class TestRequestRemote
    {
        [TestInitialize]
        public void Initialize()
        {

        }
        [TestMethod]
        public void TestSetHeader()
        {
            //Connection:keep - alive
            //Content - Length: 0
            //Accept: application / json, text / javascript, */*; q=0.01
            //Origin: http://www.oneplusbbs.com
            //X-Requested-With: XMLHttpRequest
            //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36
            //DNT: 1
            //Referer: http://www.oneplusbbs.com/plugin.php?id=choujiang
            //Accept-Encoding: gzip, deflate
            //Accept-Language: zh-CN,zh;q=0.8
            //Cookie: qKc3_0e8d_saltkey=t8e69sL9; qKc3_0e8d_lastvisit=1437546176; opcid=1437549720434_2053882285; qKc3_0e8d_nofavfid=1; ONEPLUSID=8fea06a4a9d8b0a0462a00756028fac1; qKc3_0e8d_sid=IYO4Gj; qKc3_0e8d_atarget=1; qKc3_0e8d_forum_lastvisit=D_99_1438071296; qKc3_0e8d_visitedfid=2D99D52D39; qKc3_0e8d_smile=1D1; qKc3_0e8d_security_cookiereport=6fbbilXQ4fFUTOjH%2B1V9fYb1ijUzzJncz9UT6pM5RcVIfHghrg8a; qKc3_0e8d_ulastactivity=1438130752%7C0; oppt=oneplus; qKc3_0e8d_lastact=1438130978%09plugin.php%09; opuserid=1651802; opsertime=1438130978000; opsid=1438130756358_461424928; opsct=1438130752000; opbct=1438130966000; opnt=1438130978000; opstep=8; optime_browser=1438130982229; opstep_event=0; opnt_event=1438130978000
            //Host: www.oneplusbbs.com

            for (int i = 0; i < 100; i++)
            {
                //调用一加2的一个抽奖接口
                var url = "http://www.oneplusbbs.com/plugin.php?id=choujiang&do=draw";
                var cookieStr = @"qKc3_0e8d_saltkey=t8e69sL9; qKc3_0e8d_lastvisit=1437546176; opcid=1437549720434_2053882285; qKc3_0e8d_nofavfid=1; ONEPLUSID=8fea06a4a9d8b0a0462a00756028fac1; qKc3_0e8d_sid=IYO4Gj; qKc3_0e8d_atarget=1; qKc3_0e8d_forum_lastvisit=D_99_1438071296; qKc3_0e8d_visitedfid=2D99D52D39; qKc3_0e8d_smile=1D1; qKc3_0e8d_security_cookiereport=6fbbilXQ4fFUTOjH%2B1V9fYb1ijUzzJncz9UT6pM5RcVIfHghrg8a; qKc3_0e8d_ulastactivity=1438130752%7C0; oppt=oneplus; qKc3_0e8d_lastact=1438130978%09plugin.php%09; opuserid=1651802; opsertime=1438130978000; opsid=1438130756358_461424928; opsct=1438130752000; opbct=1438130966000; opnt=1438130978000; opstep=8; optime_browser=1438130982229; opstep_event=0; opnt_event=1438130978000";
                CookieContainer cc = SetCookie(cookieStr, "", new Uri(url).Host.Replace("www.", ""));
                var result = RequestUtility.HttpPost(url, cc, "", Encoding.UTF8);

                var str = JsonConvert.DeserializeObject(result);
                Trace.WriteLine(JsonConvert.SerializeObject(str));
                Thread.Sleep(3000);
                Assert.IsNotNull(result);
            }
        }


        [TestMethod]
        public void TestQianDao()
        {
            //POST http://www.oneplusbbs.com/plugin.php?id=dsu_paulsign:sign&operation=qiandao&infloat=1&inajax=1 HTTP/1.1
            //Host:www.oneplusbbs.com
            //Connection: keep - alive
            //Content - Length: 44
            //Cache - Control: max - age = 0
            //Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,*/*;q=0.8
            //Origin: http://www.oneplusbbs.com
            //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36
            //Content-Type: application/x-www-form-urlencoded
            //DNT: 1
            //Referer: http://www.oneplusbbs.com/plugin.php?id=dsu_paulsign:sign
            //Accept-Encoding: gzip, deflate
            //Accept-Language: zh-CN,zh;q=0.8
            //Cookie: qKc3_0e8d_saltkey=t8e69sL9; qKc3_0e8d_lastvisit=1437546176; opcid=1437549720434_2053882285; qKc3_0e8d_nofavfid=1; qKc3_0e8d_atarget=1; qKc3_0e8d_forum_lastvisit=D_99_1438071296; qKc3_0e8d_visitedfid=99D2D52D39; qKc3_0e8d_smile=1D1; qKc3_0e8d_sendmail=1; oppt=oneplus; ONEPLUSID=4ae818681f48e221bc7fecdf2e5794c0; qKc3_0e8d_security_cookiereport=9b334t3WvXer3us9sP0ebUyTmim6x2uoToPSqvWHKg4hO8Bv8u%2FQ; qKc3_0e8d_ulastactivity=1438305878%7C0; opuserid=1651802; opsertime=1438306019000; qKc3_0e8d_lastact=1438306019%09home.php%09spacecp; qKc3_0e8d_checkpm=1; qKc3_0e8d_noticeTitle=1; opsid=1438305866647_1486796104; opsct=1438305866647; opbct=1438305974000; opnt=1438306019000; opstep=11; optime_browser=1438306019667; opstep_event=0; opnt_event=1438306019000

            //formhash=556a16be&qdxq=kx&qdmode=3&todaysay=
            var url = " http://www.oneplusbbs.com/plugin.php?id=dsu_paulsign:sign&operation=qiandao&infloat=1&inajax=1";

            var cookieStr = @"qKc3_0e8d_saltkey=t8e69sL9; qKc3_0e8d_lastvisit=1437546176; opcid=1437549720434_2053882285; qKc3_0e8d_nofavfid=1; qKc3_0e8d_atarget=1; qKc3_0e8d_forum_lastvisit=D_99_1438071296; qKc3_0e8d_visitedfid=99D2D52D39; qKc3_0e8d_smile=1D1; qKc3_0e8d_sendmail=1; oppt=oneplus; ONEPLUSID=4ae818681f48e221bc7fecdf2e5794c0; qKc3_0e8d_security_cookiereport=9b334t3WvXer3us9sP0ebUyTmim6x2uoToPSqvWHKg4hO8Bv8u%2FQ; qKc3_0e8d_ulastactivity=1438305878%7C0; opuserid=1651802; opsertime=1438306019000; qKc3_0e8d_lastact=1438306019%09home.php%09spacecp; qKc3_0e8d_checkpm=1; qKc3_0e8d_noticeTitle=1; opsid=1438305866647_1486796104; opsct=1438305866647; opbct=1438305974000; opnt=1438306019000; opstep=11; optime_browser=1438306019667; opstep_event=0; opnt_event=1438306019000";
            var cc = SetCookie(cookieStr, url, new Uri(url).Host);
            var formData = new Dictionary<string, string>();
            formData.Add("formhash", "556a16be");
            formData.Add("qdxq", "kx");
            formData.Add("qdmode", "3");
            formData.Add("todaysay", "");

            var result = RequestUtility.HttpPost(url, cc, formData, Encoding.UTF8);

            var str = JsonConvert.DeserializeObject(result);
            Trace.WriteLine(JsonConvert.SerializeObject(str));
            Assert.AreEqual(!string.IsNullOrEmpty(result), true);
        }

        [TestMethod]
        public void TestGetCookie()
        {
            var str = "qKc3_0e8d_saltkey=t8e69sL9; qKc3_0e8d_lastvisit=1437546176;";
            var result = SplitStr(str);

            Assert.AreEqual(result["qKc3_0e8d_saltkey"], "t8e69sL9");
            Assert.AreEqual(result["qKc3_0e8d_lastvisit"], "1437546176");
        }

        /// <summary>
        /// 注意cookieStr的格式
        /// </summary>
        /// <param name="str">qKc3_0e8d_saltkey=t8e69sL9; qKc3_0e8d_lastvisit=1437546176; opcid=1437549720434_2053882285; qKc3_0e8d_nofavfid=1; qKc3_0e8d_atarget=1; qKc3_0e8d_forum_lastvisit=D_99_1438071296; qKc3_0e8d_visitedfid=99D2D52D39; qKc3_0e8d_smile=1D1; qKc3_0e8d_sendmail=1; oppt=oneplus; ONEPLUSID=4ae818681f48e221bc7fecdf2e5794c0; qKc3_0e8d_security_cookiereport=9b334t3WvXer3us9sP0ebUyTmim6x2uoToPSqvWHKg4hO8Bv8u%2FQ; qKc3_0e8d_ulastactivity=1438305878%7C0; opuserid=1651802; opsertime=1438306019000; qKc3_0e8d_lastact=1438306019%09home.php%09spacecp; qKc3_0e8d_checkpm=1; qKc3_0e8d_noticeTitle=1; opsid=1438305866647_1486796104; opsct=1438305866647; opbct=1438305974000; opnt=1438306019000; opstep=11; optime_browser=1438306019667; opstep_event=0; opnt_event=1438306019000</param>
        /// <param name="url"></param>
        private static Dictionary<string, string> SplitStr(string str)
        {
            if (str.EndsWith(";"))//因为最后的;会导致分组后多出一个空字符串组
            {
                str = str.Substring(0, str.Length - 1);
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] strs = str.Split(';');
            foreach (var item in strs)
            {
                try
                {
                    string[] items = item.Split('=');
                    dic.Add(items[0].Trim(), items[1].Trim());
                }
                catch
                {
                    throw new ArgumentException("格式错误,键值对必须以=连接");
                }
            }
            return dic;
        }

        private static CookieContainer SetCookie(string cookieStr, string path, string domain)
        {
            var dic = SplitStr(cookieStr);
            return SetCookie(dic, path, domain);
        }

        private static CookieContainer SetCookie(Dictionary<string, string> dic, string path, string domain)
        {
            CookieContainer cc = new CookieContainer(300, 50, 4096);
            foreach (var item in dic)
            {
                cc.Add(new Cookie(item.Key, item.Value, "", "oneplusbbs.com"));
            }
            return cc;
        }
    }
}
