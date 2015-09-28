using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.Helpers
{
    internal class CommonHelper
    {
        public static string GetTimeStamp()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }

        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(str));
            string str2 = "";
            for (int i = 0; i < result.Length; i++)
            {
                str2 += string.Format("{0:x}", result[i]);
            }
            return str2;
        }

        public static string Sha1(string str)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);
            //Hash运算
            byte[] dataHashed = sha1.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "").ToLower();
            return hash;
        }

        public static string GuidTo16String(Guid ID)
        {
            long i = 1;
            foreach (byte b in ID.ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}