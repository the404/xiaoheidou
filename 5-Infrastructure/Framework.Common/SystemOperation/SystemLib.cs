using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.SystemOperation
{
    public class SystemLib
    {
        #region 目录

        /// <summary>
        /// 以程序根目录的相对路径拼接绝对路径
        /// </summary>
        /// <param name="pathStr">文件夹名数组</param>
        /// <returns>以"\"结尾</returns>
        public static string GetAppRootDir(params string[] pathStr)
        {
            string appRootDir = AppDomain.CurrentDomain.BaseDirectory;
            appRootDir = appRootDir.EndsWith("\\") ? appRootDir : appRootDir + "\\";
            appRootDir += CombineStrings("\\", pathStr);
            return appRootDir;
        }

        private static string CombineStrings(string splitString, params string[] args)
        {
            string result = "";
            foreach (var item in args)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result += item + splitString;
                }
            }
            return result;
        }

        #endregion
    }
}
