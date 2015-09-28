using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace EasyWeixin.Core.Common
{
    public class Function
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

        private static string GetCookie(string CookieStr)
        {
            string result = "";

            string[] myArray = CookieStr.Split(',');

            if (myArray.Count() > 0)
            {
                result = "Cookie: ";

                foreach (var str in myArray)
                {
                    string[] CookieArray = str.Split(';');

                    result += CookieArray[0].Trim();

                    result += "; ";
                }

                result = result.Substring(0, result.Length - 2);
            }

            return result;
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetRandomString(int num)
        {
            string str = "";
            Random rnd = new Random();
            for (int i = 0; i < num; i++)
            {
                str += GetChar(rnd);
            }
            return str;
        }

        /// <summary>
        /// 获取单个随机字符 数字 大写字母 小写字母
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static string GetChar(Random rnd)
        {
            // 0-9
            // A-Z  ASCII值  65-90
            // a-z  ASCII值  97-122
            int i = rnd.Next(0, 123);
            if (i < 10)
            {
                // 返回数字
                return i.ToString();
            }
            char c = (char)i;
            // 返回大小写字母加数字
            return char.IsLetter(c) ? c.ToString() : GetChar(rnd);
        }

        /// <summary>
        /// 根据GUID获取16位的唯一字符串
        /// </summary>
        /// <param name=\"guid\"></param>
        /// <returns></returns>
        public static string GuidTo16String(Guid ID)
        {
            long i = 1;
            foreach (byte b in ID.ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列
        /// </summary>
        /// <returns></returns>
        public static long GuidToLongID(Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 生成22位随机码
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueId()
        {
            Thread.Sleep(1);
            Random random = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            string strUnique = DateTime.Now.ToString("yyyyMMddHHmmssffff") + random.Next(1000, 9999);
            return strUnique;
        }
    }
}