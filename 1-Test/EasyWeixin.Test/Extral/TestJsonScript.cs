using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EasyWeixin.Test.Extral
{
    /// <summary>
    /// 在json中添加html+js的组合代码
    /// </summary>
    [TestClass]
    public class TestJsonScript
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = @"< script type = 'text/javascript' reload = '1' >
                           setTimeout('hideWindow('qwindow')', 3000);
                        </ script >
                        < div class='f_c'>
                        <h3 class='flb'>
                        <em id = 'return_win' > 签到提示 </ em >
                        < span >
                        < a href='javascript:;' class='flbc' onclick='hideWindow('qwindow')' title='关闭'>关闭</a></span>
                        </h3>
                        <div class='c'>
                        您今日已经签到，请明天再来！ </div>
                        </div>";
            var obj = new { root = str.Replace("\r\n", "") };
            var result = JsonConvert.SerializeObject(obj);
            Trace.WriteLine(result);
        }
    }
}
