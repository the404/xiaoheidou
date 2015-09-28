using System;
using System.Linq;
using System.Text;

namespace EasyWeixin
{
    public class CheckSignature
    {
        /// <summary>
        /// 在网站没有提供Token（或传入为null）的情况下的默认Token，建议在网站中进行配置。
        /// </summary>
        public const string Token = "weixin";

        /// <summary>
        /// 判断消息的真实性
        /// 检查签名是否正确
        /// 判断签名的逻辑就是利用参数后台再自己按照逻辑来生成以便signature然后对比
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool Check(string signature, string timestamp, string nonce, string token = null)
        {
            return signature == GetSignature(timestamp, nonce, token);
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce, string token = null)
        {
            token = token ?? Token;
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            //var enText = FormsAuthentication.HashPasswordForStoringInConfigFile(arrString, "SHA1");
            //使用System.Web.Security程序集
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }

        /// <summary>
        /// 将数组转成字符串
        /// </summary>
        /// <param name="glue">分隔符</param>
        /// <param name="pieces">要字符串数组</param>
        public static string Implode(string glue, string[] pieces)
        {
            string result = string.Empty;
            int count = pieces.Length;
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    result = pieces[i];
                }
                else
                {
                    result = result + glue.ToString() + pieces[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 生成sha1散列
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string sha1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        public static string MD5ForPHP(string stringToHash)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] emailBytes = Encoding.UTF8.GetBytes(stringToHash.ToLower());
            byte[] hashedEmailBytes = md5.ComputeHash(emailBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashedEmailBytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }
}