using System;
using EasyWeixin.Core.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.ShortUrl;

namespace EasyWeixin.Test.ShortUrl
{
    [TestClass]
    public class ShortUrlhelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //压根就不能将两个参数的地址压缩成短链接,因为&被转换系统默认使用了,所以不能正确解析

            string longUrl = "http://wx.smartoct.com/News/GuessNews?User_ID=622a17d7-355a-4d59-af18-416c3bfba132&ImageTextID=b9e669ba-adaf-4fd2-930a-502e66633685";
            var resultBaidu = ShortUrlhelper.GetShortUrl_Baidu(longUrl);
            var resultSina = GetShortUrl.GetSina(longUrl);

        }
    }
}
