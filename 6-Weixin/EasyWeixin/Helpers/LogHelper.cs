using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;

namespace EasyWeixin.Helpers
{
    public static class LogHelper
    {
        private const string IsWriteLog = "IsWriteLog";
        /// <summary>
        /// 当前所在的主机+端口 例如:localhost:11852/
        /// </summary>        
        public const string LogMark = "release";
        private static object _obj = new object();

        /// <summary>
        /// 记录异常,这个是不管如何都写出来的
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="path"></param>
        /// <param name="fileName">默认是当前时间+guid</param>
        public static void WriteException(Exception ex, string path = @"/bin/logs/exception")
        {
            lock (_obj)
            {
                try
                {
                    if (!path.Contains("//")) //相对路径
                    {
                        path = HttpContext.Current.Server.MapPath(path);
                    }
                    //如果可以将日志文件加上methodname的话,应该是不错的

                    var title = ex.Message.Length > 50 ? ex.Message.Substring(50) : ex.Message;
                    var fileName = title + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".json";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (StreamWriter sw = new StreamWriter(path + "\\" + fileName, true, Encoding.UTF8))
                    {
                        JsonSerializerSettings setting = new JsonSerializerSettings();
                        setting.NullValueHandling = NullValueHandling.Ignore;

                        var str = JsonConvert.SerializeObject(ex, Formatting.Indented, setting);
                        sw.WriteLine(str);
                    }
                }
                catch (Exception)
                {
                    //这里一定是参数中的异常，而不是该方法中的异常
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 记录普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static void WriteLog(string message, string fileName = "", string path = @"/bin/logs/log")
        {
            lock (_obj)
            {
                //也就是通过这一个字段来控制所有的常规的日志在服务器上不输出
                if (!CheckIsWriteLog(fileName))
                {
                    return;
                }

                if (!path.Contains("//")) //相对路径
                {
                    path = HttpContext.Current.Server.MapPath(path);
                }

                fileName += DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter sw = new StreamWriter(path + "\\" + fileName, true, Encoding.UTF8))
                {
                    sw.WriteLine("|---------------------------------------------------------------|");
                    sw.WriteLine("|--------------------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "--------------------|");
                    sw.WriteLine("|---------------------------------------------------------------|");
                    sw.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// 与之前方法的区别是filename不会添加一个当前的时间作为区分
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        public static void Log(string message, string fileName, string path = @"/bin/logs/log")
        {
            try
            {
                lock (_obj)
                {
                    if (!CheckIsWriteLog(fileName))
                    {
                        return;
                    }

                    if (!path.Contains("//")) //相对路径
                    {
                        path = HttpContext.Current.Server.MapPath(path);
                    }

                    fileName += ".txt";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (StreamWriter sw = new StreamWriter(path + "\\" + fileName, true, Encoding.UTF8))
                    {
                        sw.WriteLine("|---------------------------------------------------------------|");
                        sw.WriteLine("|--------------------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "--------------------|");
                        sw.WriteLine("|---------------------------------------------------------------|");
                        sw.WriteLine(message);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private static bool CheckIsWriteLog(string fileName)
        {
            if (fileName.Contains(LogMark))//包含release的日志,不需要根据web.config的配置来决定是否写日志
            {
                return true;
            }
            bool flag = false;
            var result = Boolean.TryParse(ConfigurationManager.AppSettings[IsWriteLog], out flag);
            if (!result) //不写的时候为null,则转化失败,result=false
            {
                return false;
            }
            else //如果能够转化,那么根据具体配置来
            {
                if (!Boolean.Parse(ConfigurationManager.AppSettings[IsWriteLog]))//如果配置为false
                    return false;
            }
            return true;
        }
    }
}