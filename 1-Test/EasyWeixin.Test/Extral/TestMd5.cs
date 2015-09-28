using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Diagnostics;

namespace EasyWeixin.Test.Extral
{
    [TestClass]
    public class TestMd5
    {
        string a = "11111";
        [TestMethod]
        public void Test16()
        {
            var start = Encoding.Default.GetBytes(a);
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var result = md5.ComputeHash(start);
            var b = BitConverter.ToString(result, 4, 8).Replace("-", "");
            Assert.AreEqual(b.Length, 16);
        }
        [TestMethod]
        public void Test32()
        {
            var start = Encoding.Default.GetBytes(a);
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var result = md5.ComputeHash(start);
            foreach (var item in result)
                Trace.WriteLine(item);
            var b = BitConverter.ToString(result).Replace("-", "");
            Trace.WriteLine(b);
            Assert.AreEqual(b.Length, 32);
            Trace.WriteLine("就是将result这个byte[]中每一个字段转化成两位的16进制");
        }
    }
}
