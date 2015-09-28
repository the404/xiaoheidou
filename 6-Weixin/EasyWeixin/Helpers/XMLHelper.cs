using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace EasyWeixin.Helpers
{
    public static class XMLHelper
    {
        /// <summary>
        /// 创建一个空的XML文件,若此文件已存在，将会创建一个新文件覆盖原文件
        /// </summary>
        /// <param name="Path">XML文件保存路径</param>
        /// <param name="Name">XML文件名称</param>
        /// <param name="Message">错误信息</param>
        /// <returns>True Or False</returns>
        public static bool CreateXML(string Path, string Name, out string Message)
        {
            bool Result = false;
            Message = string.Empty;
            StreamWriter sw = null;
            if (Path.Length > 0)
            {
                try
                {
                    Path = HttpContext.Current.Server.MapPath(Path);
                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path);
                    }
                    if (Name.Length > 0)
                    {
                        sw = new StreamWriter(Path + Name + ".xml", false, Encoding.UTF8);
                        sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?> ");
                    }
                    else
                    {
                        Message = "XML文件名称不能为空，无法创建XML文件";
                    }
                }
                catch (Exception ex)
                {
                    //LogHelper.AddErrorLog(ex);
                    Message = "创建XML文件时发生错误，错误原因："
                        + ex.Message + ex.ToString().Substring(0, ex.ToString().IndexOf(":")) + "。详细错误原因请查看错误日志。";
                    if (sw != null) sw.Dispose();
                }
                finally
                {
                    if (sw != null) sw.Dispose();
                }
            }
            else
            {
                Message = "XML文件路径不能为空，无法创建XML文件";
            }

            return Result;
        }

        /// <summary>
        /// 加载XML文档
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Message">错误信息</param>
        /// <returns>XmlDocument</returns>
        public static XmlDocument LoadXmlDocument(string Path, out string Message)
        {
            Message = string.Empty;
            XmlDocument doc = new XmlDocument();
            if (Path.Length > 0)
            {
                Path = HttpContext.Current.Server.MapPath(Path);
            }
            else
            {
                Message = "XML文件路径不能为空，无法处理XML文件";
            }
            if (File.Exists(Path))
            {
                doc.Load(Path);
            }
            else
            {
                Message = "配置文件不存在，无法处理信息";
            }
            return doc;
        }

        /// <summary>
        /// 读取html，返回对应源代码
        /// </summary>
        /// <param name="Path">html路径(物理文件路径)</param>
        /// <param name="str">处理后的字符串</param>
        /// <returns></returns>
        public static bool LoadHtmlString(string Path, out string str)
        {
            str = string.Empty;
            bool result = true;
            FileStream stream = null;
            StreamReader reader = null;
            try
            {
                stream = new FileStream(Path, FileMode.Open);
                reader = new StreamReader(stream, Encoding.UTF8);
                str = reader.ReadToEnd();
            }
            catch (Exception)
            {
                //LogHelper.AddErrorLog(ex);
                result = false;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
            }
            return result;
        }
    }
}