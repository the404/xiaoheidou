using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyWeixin.Test.Extral
{
    [TestClass]
    public class TestString
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = "114352833";
            var str2 = "114352833";
            Assert.AreEqual(true, str.Contains(str2));
        }
    }
}
