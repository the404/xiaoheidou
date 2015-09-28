using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace EasyWeixin.Helpers
{
    public class ImageHelper
    {
        /// <summary>
        /// 小缩略图的宽
        /// </summary>
        public int ThumbWidth { get; set; }

        /// <summary>
        /// 大缩略图的宽
        /// </summary>
        public int WaterWidth { get; set; }

        /// <summary>
        /// 置顶缩略图的宽
        /// </summary>
        public int TopWidth { get; set; }

        /// <summary>
        /// 是否保存原图
        /// </summary>
        public int IsSave { get; set; }

        /// <summary>
        /// 原图上传最大尺寸
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// 小缩略图保存地址
        /// </summary>
        public string ThumbSavePath { get; set; }

        /// <summary>
        /// 大缩略图保存地址
        /// </summary>
        public string WaterSavePath { get; set; }

        /// <summary>
        /// 原图保存地址
        /// </summary>
        public string OrginalSavePath { get; set; }

        public ImageHelper()
        {
        }

        public ImageHelper(string Path)
        {
            string Message = string.Empty;
            XmlDocument doc = XMLHelper.LoadXmlDocument(Path, out Message);
            ThumbWidth = Convert.ToInt32(doc.SelectSingleNode("/image/thumb/width").InnerText);
            ThumbSavePath = doc.SelectSingleNode("/image/thumb/savepath").InnerText;
            WaterWidth = Convert.ToInt32(doc.SelectSingleNode("/image/water/width").InnerText);
            WaterSavePath = doc.SelectSingleNode("/image/water/savepath").InnerText;
            IsSave = Convert.ToInt32(doc.SelectSingleNode("/image/orginal/issave").InnerText);
            OrginalSavePath = doc.SelectSingleNode("/image/orginal/savepath").InnerText;
            MaxSize = Convert.ToInt32(doc.SelectSingleNode("/image/maxsize").InnerText);
        }

        /// <summary>
        /// 扑捉内容区图片，并将图片重命名后转移到置顶目录，并用新地址替换源地址
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="filePath">转移后的图片路径</param>
        /// <returns></returns>
        public static string MoveImage(string Content, string filePath)
        {
            string OriginalSrc = "";
            string reg = "<img[ \f\n\r\t\v]*[^>]*/?>";
            Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
            MatchCollection array = regex.Matches(Content);
            for (int i = 0; i < array.Count; i++)
            {
                string img = array[i].Value;
                if (img.IndexOf("http://") >= 0 || img.IndexOf("https://") >= 0 || img.IndexOf("ftp:") >= 0 || img.IndexOf("file:") >= 0)
                {
                    continue;
                }
                /*
                * 获取图片的Url
                */
                regex = new Regex("src=\"[^ \f\n\r\t\v][^\"]*\"", RegexOptions.IgnoreCase);
                int a = regex.Match(img).ToString().IndexOf('"') + 1;
                int b = regex.Match(img).ToString().LastIndexOf('"');
                OriginalSrc = regex.Match(img).ToString().Substring(a, b - a);

                /*
                 * 移动文件到指定目录
                 */
                if (File.Exists(HttpContext.Current.Server.MapPath(OriginalSrc)))
                {
                    int index = OriginalSrc.LastIndexOf(".");
                    string fileType = OriginalSrc.Substring(index).ToLower();
                    int index2 = OriginalSrc.LastIndexOf("/");
                    Random r = new Random();
                    string StringRandom = r.Next(999999999).ToString() + i; ;
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + StringRandom + fileType;
                    try
                    {
                        if (OriginalSrc.IndexOf("uploadfiles") == -1)
                        {
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath(filePath)))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(filePath));
                            }
                            FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(OriginalSrc));
                            File.Move(HttpContext.Current.Server.MapPath(OriginalSrc), HttpContext.Current.Server.MapPath(filePath) + fileName);
                            Content = Content.Replace(OriginalSrc, filePath + fileName);
                        }
                    }
                    catch (Exception)
                    {
                        //LogHelper.AddErrorLog(ex);
                    }
                }
            }
            return Content;
        }

        /// <summary>
        /// 扑捉内容区图片，并将图片重命名后转移到指定目录，并用新地址替换原地址，如果捕捉到的图片超过设定的大小，则不转移图片，同时生成缩略图到指定目录
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="filePath">转移后的图片路径</param>
        /// <param name="Width">要生成的缩略图宽</param>
        /// <param name="Height">要生成的缩略图高</param>
        /// <param name="Height">设定图片大小，以字节为单位（1MB=1024KB=1024*1024B）</param>
        /// <returns></returns>
        public static string MoveImage(string Content, string filePath, int Width, int Length)
        {
            string TempContent = Content;
            string OriginalSrc = string.Empty;
            string reg = "<img[ \f\n\r\t\v]*[^>]*/?>";
            Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
            MatchCollection array = regex.Matches(TempContent);
            for (int i = 0; i < array.Count; i++)
            {
                string img = array[i].Value;
                if (img.IndexOf("http://") >= 0 || img.IndexOf("https://") >= 0 || img.IndexOf("ftp:") >= 0 || img.IndexOf("file:") >= 0)
                {
                    continue;
                }
                /*
                * 获取图片的Url
                */
                regex = new Regex("src=\"[^ \f\n\r\t\v][^\"]*\"", RegexOptions.IgnoreCase);
                int a = regex.Match(img).ToString().IndexOf('"') + 1;
                int b = regex.Match(img).ToString().LastIndexOf('"');
                OriginalSrc = regex.Match(img).ToString().Substring(a, b - a);

                /*
                 * 移动文件到指定目录
                 */
                if (File.Exists(HttpContext.Current.Server.MapPath(OriginalSrc)))
                {
                    int index = OriginalSrc.LastIndexOf(".");
                    string fileType = OriginalSrc.Substring(index).ToLower();
                    int index2 = OriginalSrc.LastIndexOf("/");
                    Random r = new Random();
                    string StringRandom = r.Next(999999999).ToString() + i; ;
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + StringRandom + fileType;
                    try
                    {
                        if (OriginalSrc.ToLower().IndexOf("uploadfiles") == -1)
                        {
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath(filePath)))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(filePath));
                            }
                            FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(OriginalSrc));
                            if (file.Length > Length)
                            {
                                TempContent = TempContent.Replace(OriginalSrc, GetThumbImg(Width, OriginalSrc, "1"));
                            }
                            else
                            {
                                File.Copy(HttpContext.Current.Server.MapPath(OriginalSrc), HttpContext.Current.Server.MapPath(filePath) + fileName);
                                TempContent = TempContent.Replace(OriginalSrc, filePath + fileName);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        TempContent = Content;
                        // LogHelper.AddErrorLog("将新闻图片转移时发生错误！错误源：ImageHelper/MoveImage(string,string,int,int)。错误信息：" + ex.Message);
                    }
                }
            }
            return TempContent;
        }

        /// <summary>
        /// 根据原图地址和缩略图宽获取等比的缩略图高
        /// </summary>
        /// <param name="filePath">原图地址（相对路径）</param>
        /// <param name="Width">目标缩略图宽</param>
        /// <returns>等比缩略图高</returns>
        public static int GetImageHeight(string filePath, int Width)
        {
            int Height = 0;
            System.Drawing.Image oImage = null;
            try
            {
                oImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(filePath));
                Height = (int)Math.Floor(Convert.ToDouble(oImage.Height) * (Convert.ToDouble(Width) / Convert.ToDouble(oImage.Width)));
            }
            catch (Exception)
            {
                // LogHelper.AddErrorLog(ex);
            }
            finally
            {
                if (oImage != null) oImage.Dispose();
            }
            return Height;
        }

        /// <summary>
        /// 根据原图地址和缩略图宽生成缩略图
        /// </summary>
        /// <param name="width">小图宽</param>
        /// <param name="height">小图高</param>
        /// <param name="OriginalSrc">原图相对路径</param>
        /// <returns>小图保存路径</returns>
        public static string GetThumbImg(int width, string OriginalSrc, string act)
        {
            string ThumbImg = string.Empty;
            //原图绝对路径
            string src = HttpContext.Current.Server.MapPath(OriginalSrc);
            /*
             *生成图片缩略图
             */
            if (File.Exists(src))
            {
                int index = OriginalSrc.LastIndexOf(".");
                string fileType = OriginalSrc.Substring(index).ToLower();
                int index2 = OriginalSrc.LastIndexOf("/");
                string filePath = OriginalSrc.Substring(0, index2 + 1);

                int start = OriginalSrc.LastIndexOf('/');
                int end = OriginalSrc.IndexOf('.');
                //原图名称
                string OriginalImgName = OriginalSrc.Substring(start + 1, end - start - 1);
                //小图名称
                string ThumbImgName = "small_" + OriginalImgName;

                //原图缩约图
                string YThumbImgName = "ysmall_" + OriginalImgName;

                //如果存在此缩略图
                //if (File.Exists(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType)))
                //{
                //    ThumbImg = filePath + ThumbImgName + fileType;
                //}
                //else
                //{
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(filePath)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(filePath));
                }
                System.Drawing.Image oImage = System.Drawing.Image.FromFile(src);
                int owidth = oImage.Width; //原图宽度
                int oheight = oImage.Height; //原图高度
                int width2 = width;
                int height2 = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(width2) / Convert.ToDouble(owidth)));//等比设定高度
                                                                                                                                 //生成缩略原图 （大图）
                Bitmap sImage = new Bitmap(width2, height2);
                Graphics sg = Graphics.FromImage(sImage);
                sg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                sg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                sg.Clear(Color.Transparent); //清空画布并以透明背景色填充
                sg.DrawImage(oImage, new Rectangle(0, 0, width2, height2), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);
                try
                {
                    if (act == "1")
                    {
                        sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Bmp);
                        ThumbImg = filePath + ThumbImgName + fileType;
                    }
                    else
                    {
                        sImage.Save(HttpContext.Current.Server.MapPath(filePath + YThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Bmp);
                        ThumbImg = filePath + YThumbImgName + fileType;
                    }
                }
                catch (Exception)
                {
                    // LogHelper.AddErrorLog(ex);
                }
                finally
                {
                    //释放资源
                    sg.Dispose();
                    sImage.Dispose();
                    oImage.Dispose();
                }
                //}
            }
            return ThumbImg;
        }

        /// <summary>
        /// 根据设定的宽高生成一张缩略图
        /// </summary>
        /// <param name="file">上传的图片文件</param>
        /// <param name="ImgName">out 处理后的小图名称</param>
        /// <param name="message">out 错误信息</param>
        /// <param name="Thumb_SavePath">小图保存路径</param>
        /// <param name="height">小图高</param>
        /// <param name="width">小图宽</param>
        /// <returns>True Or False 处理成功或失败</returns>
        public static bool GetThumbImg(FileUpload file, out string ImgName, out string Message, string ThumbSavePath, int height, int width)
        {
            ImgName = string.Empty;
            Message = string.Empty;
            bool Result = true;
            Stream oStream = null;
            System.Drawing.Image oImage = null;
            Bitmap tImage = null;
            Graphics g = null;
            string filename = file.FileName.ToLower();
            int i = filename.LastIndexOf(".");
            filename = filename.Substring(i).ToLower();
            if (!(filename == ".bmp" || filename == ".jpeg" || filename == ".jpg" || filename == ".png"))
            {
                Message = "请上传jpg/bmp/png格式的图片!";
                return false;
            }
            //生成6位随机数字
            Random r = new Random();
            string StringRandom = r.Next(999999).ToString();
            //格式化日期作为文件名
            string StringTime = DateTime.Now.ToString("yyMMdd");
            ImgName = StringTime + StringRandom + filename;
            try
            {
                ThumbSavePath = HttpContext.Current.Server.MapPath(ThumbSavePath);
                if (!Directory.Exists(ThumbSavePath))
                {
                    Directory.CreateDirectory(ThumbSavePath);//在根目录下建立文件夹
                }
                //生成原图
                oStream = file.PostedFile.InputStream;
                oImage = System.Drawing.Image.FromStream(oStream);
                int owidth = oImage.Width; //原图宽度
                int oheight = oImage.Height; //原图高度
                if (owidth > width)//原图尺寸大于小缩略图尺寸
                {
                    height = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(width) / Convert.ToDouble(owidth)));//等比设定高度
                    tImage = new Bitmap(width, height);
                    g = Graphics.FromImage(tImage);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                    g.Clear(Color.Transparent); //清空画布并以透明背景色填充
                    g.DrawImage(oImage, new Rectangle(0, 0, width, height), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);
                    //开始保存图片至服务器
                    switch (filename)
                    {
                        case ".jpeg":
                        case ".jpg":
                            {
                                tImage.Save(ThumbSavePath + ImgName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;
                            }
                        case ".png":
                            {
                                tImage.Save(ThumbSavePath + ImgName, System.Drawing.Imaging.ImageFormat.Png);
                                break;
                            }
                        case ".bmp":
                            {
                                tImage.Save(ThumbSavePath + ImgName, System.Drawing.Imaging.ImageFormat.Bmp);
                                break;
                            }
                    }
                }
                else
                {
                    file.SaveAs(ThumbSavePath + ImgName);
                }
                Result = true;
            }
            catch (Exception ex)
            {
                // LogHelper.AddErrorLog(ex);
                Message = "处理图片出错，错误原因：" + ex.Message + "。详细错误信息请查看错误日志。";
                Result = false;
            }
            finally
            {
                //释放资源
                if (oImage != null) oImage.Dispose();
                if (g != null) g.Dispose();
                if (tImage != null) tImage.Dispose();
            }
            return Result;
        }

        /// <summary>
        /// 上传HttpPostedFile图片,生成两张缩略图
        /// </summary>
        /// <param name="file">System.Web.HttpPostedFile</param>
        /// <param name="imgName">out imgName/返回处理后的图片（原图）名称</param>
        /// <param name="message">out message/返回错误信息</param>
        /// <param name="thumbSavePath">小缩略图保存路径</param>
        /// <param name="waterSavePath">大缩略图保存路径</param>
        /// <param name="originalSavePath">原图保存路径</param>
        /// <param name="thumbWidth">小缩略图宽</param>
        /// <param name="thumbHeight">小缩略图高</param>
        /// <param name="isSave">是否保存原图，1：保存，0：不保存</param>
        /// <returns>返回处理结果，true为成功，false为失败</returns>
        public static bool HttpPostedFileUpload(HttpPostedFile file, out string fileName, out string message, string thumbSavePath, string waterSavePath,
            string orginalSavePath, int thumbWidth, int waterWidth, int isSave)
        {
            message = string.Empty;
            bool Result = false;
            //文件后缀名
            string fileType = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
            if (!(fileType == ".jpg" || fileType == ".jpeg" || fileType == ".pjpeg" || fileType == ".bmp"))
            {
                message = "只能上传jpg和bmp格式的图片！";
                fileName = string.Empty;
                return false;
            }
            Bitmap tImage = null;
            Graphics g = null;
            Bitmap sImage = null;
            Graphics sg = null;
            Stream oStream = null;
            System.Drawing.Image oImage = null;

            //文件名称
            DateTime dt = DateTime.Now;
            Random rd = new Random();
            fileName = dt.ToString("yyyyMMddHHmm") + rd.Next(999999).ToString() + fileType;

            try
            {
                //文件保存路径
                thumbSavePath = HttpContext.Current.Server.MapPath(thumbSavePath);
                if (!Directory.Exists(thumbSavePath))
                {
                    Directory.CreateDirectory(thumbSavePath);
                }
                waterSavePath = HttpContext.Current.Server.MapPath(waterSavePath);
                if (!Directory.Exists(waterSavePath))
                {
                    Directory.CreateDirectory(waterSavePath);
                }
                //是否保存原图
                if (isSave == 1)
                {
                    orginalSavePath = HttpContext.Current.Server.MapPath(orginalSavePath);
                    if (!Directory.Exists(orginalSavePath))
                    {
                        Directory.CreateDirectory(orginalSavePath);
                    }
                    file.SaveAs(orginalSavePath + fileName);//保存原图
                }
                //生成原图
                oStream = file.InputStream;
                oImage = System.Drawing.Image.FromStream(oStream);
                int owidth = oImage.Width; //原图宽度
                int oheight = oImage.Height; //原图高度

                //处理小缩略图
                if (owidth > thumbWidth)//原图尺寸大于小缩略图尺寸
                {
                    int thumbHeight = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(thumbWidth) / Convert.ToDouble(owidth)));//等比设定高度
                                                                                                                                             //生成缩略原图 (小图)
                    tImage = new Bitmap(thumbWidth, thumbHeight);
                    g = Graphics.FromImage(tImage);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                    g.Clear(Color.Transparent); //清空画布并以透明背景色填充
                    g.DrawImage(oImage, new Rectangle(0, 0, thumbWidth, thumbHeight), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);
                    tImage.Save(thumbSavePath + "T" + fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else
                {
                    file.SaveAs(thumbSavePath + "T" + fileName);//保存小缩略图
                }
                if (owidth > waterWidth)
                {
                    //生成缩略原图 （大图）
                    int waterHeight = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(waterWidth) / Convert.ToDouble(owidth)));//等比设定高度
                    sImage = new Bitmap(waterWidth, waterHeight);
                    sg = Graphics.FromImage(sImage);
                    sg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                    sg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                    sg.Clear(Color.Transparent); //清空画布并以透明背景色填充
                    sg.DrawImage(oImage, new Rectangle(0, 0, waterWidth, waterHeight), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);
                    sImage.Save(waterSavePath + "W" + fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else
                {
                    file.SaveAs(waterSavePath + "W" + fileName);//保存大缩略图
                }
                Result = true;
            }
            catch (Exception ex)
            {
                //LogHelper.AddErrorLog(ex);
                message = ex.Message;
                Result = false;
            }
            finally
            {
                if (tImage != null) tImage.Dispose();
                if (sImage != null) sImage.Dispose();
                if (g != null) g.Dispose();
                if (sg != null) sg.Dispose();
                if (oStream != null) oStream.Dispose();
                if (oImage != null) oImage.Dispose();
            }
            return Result;
        }

        /// <summary>
        /// 上传FileUpload图片,生成两张缩略图
        /// </summary>
        /// <param name="file">System.Web.HttpPostedFile</param>
        /// <param name="imgName">out imgName/返回处理后的图片（原图）名称</param>
        /// <param name="message">out message/返回错误信息</param>
        /// <param name="thumbSavePath">小缩略图保存路径</param>
        /// <param name="waterSavePath">大缩略图保存路径</param>
        /// <param name="originalSavePath">原图保存路径</param>
        /// <param name="thumbWidth">小缩略图宽</param>
        /// <param name="thumbHeight">小缩略图高</param>
        /// <param name="isSave">是否保存原图，1：保存，0：不保存</param>
        /// <returns>返回处理结果，true为成功，false为失败</returns>
        public static bool FileUpload(FileUpload file, out string fileName, out string message, string thumbSavePath, string waterSavePath,
            string orginalSavePath, int thumbWidth, int waterWidth, int isSave)
        {
            message = string.Empty;
            bool Result = false;
            //文件后缀名
            string fileType = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
            //文件名称
            DateTime dt = DateTime.Now;
            Random rd = new Random();
            fileName = dt.ToString("yyyyMMddHHmm") + rd.Next(999999).ToString() + fileType;
            Bitmap tImage = null;
            Graphics g = null;
            Bitmap sImage = null;
            Graphics sg = null;
            Stream oStream = null;
            System.Drawing.Image oImage = null;
            try
            {
                //文件保存路径
                thumbSavePath = HttpContext.Current.Server.MapPath(thumbSavePath);
                if (!Directory.Exists(thumbSavePath))
                {
                    Directory.CreateDirectory(thumbSavePath);
                }
                waterSavePath = HttpContext.Current.Server.MapPath(waterSavePath);
                if (!Directory.Exists(waterSavePath))
                {
                    Directory.CreateDirectory(waterSavePath);
                }
                //是否保存原图
                if (isSave == 1)
                {
                    orginalSavePath = HttpContext.Current.Server.MapPath(orginalSavePath);
                    if (!Directory.Exists(orginalSavePath))
                    {
                        Directory.CreateDirectory(orginalSavePath);
                    }
                    file.SaveAs(orginalSavePath + fileName);//保存原图
                }
                //生成原图
                oStream = file.PostedFile.InputStream;
                oImage = System.Drawing.Image.FromStream(oStream);
                int owidth = oImage.Width; //原图宽度
                int oheight = oImage.Height; //原图高度

                //处理小缩略图
                if (owidth > thumbWidth)//原图尺寸大于小缩略图尺寸
                {
                    int thumbHeight = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(thumbWidth) / Convert.ToDouble(owidth)));//等比设定高度
                                                                                                                                             //生成缩略原图 (小图)
                    tImage = new Bitmap(thumbWidth, thumbHeight);
                    g = Graphics.FromImage(tImage);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                    g.Clear(Color.Transparent); //清空画布并以透明背景色填充
                    g.DrawImage(oImage, new Rectangle(0, 0, thumbWidth, thumbHeight), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);
                    switch (fileType)
                    {
                        case ".pjpeg":
                        case ".jpeg":
                        case ".jpg":
                            {
                                tImage.Save(thumbSavePath + "T" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;
                            }
                        case ".bmp":
                            {
                                tImage.Save(thumbSavePath + "T" + fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                break;
                            }
                    }
                }
                else
                {
                    file.SaveAs(thumbSavePath + "T" + fileName);//保存小缩略图
                }
                if (owidth > waterWidth)
                {
                    //生成缩略原图 （大图）
                    int waterHeight = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(waterWidth) / Convert.ToDouble(owidth)));//等比设定高度
                    sImage = new Bitmap(waterWidth, waterHeight);
                    sg = Graphics.FromImage(sImage);
                    sg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                    sg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                    sg.Clear(Color.Transparent); //清空画布并以透明背景色填充
                    sg.DrawImage(oImage, new Rectangle(0, 0, waterWidth, waterHeight), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);
                    switch (fileType)
                    {
                        case ".pjpeg":
                        case ".jpeg":
                        case ".jpg":
                            {
                                sImage.Save(waterSavePath + "W" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;
                            }
                        case ".bmp":
                            {
                                sImage.Save(waterSavePath + "W" + fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                break;
                            }
                    }
                }
                else
                {
                    file.SaveAs(waterSavePath + "W" + fileName);//保存大缩略图
                }
                Result = true;
            }
            catch (Exception ex)
            {
                // LogHelper.AddErrorLog(ex);
                message = ex.Message;
                Result = false;
            }
            finally
            {
                if (tImage != null) tImage.Dispose();
                if (sImage != null) sImage.Dispose();
                if (g != null) g.Dispose();
                if (sg != null) sg.Dispose();
                if (oStream != null) oStream.Dispose();
                if (oImage != null) oImage.Dispose();
            }
            return Result;
        }

        /// <summary>
        /// 返回内容区第一张图片的缩略图
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="FilePath">当前文件完整目录</param>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">高度</param>
        /// <param name="IsFilter">是否根据设定宽高比进行筛选</param>
        /// <returns></returns>
        public static string GetThumbImg(string Content, string FilePath, int width, int height, bool IsFilter)
        {
            System.Drawing.Image oImage = null;
            Bitmap sImage = null;
            Graphics sg = null;
            bool result = true;
            //设定宽高比
            double Filter = Convert.ToDouble(width) / height;
            string ThumbImg = string.Empty;
            string reg = "<img[ \f\n\r\t\v]*[^>]*/?>";
            Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
            MatchCollection array = regex.Matches(Content);
            for (int i = 0; i < array.Count; i++)
            {
                string img = array[i].Value;
                if (img.IndexOf("http://") >= 0 || img.IndexOf("https://") >= 0 || img.IndexOf("ftp:") >= 0 || img.IndexOf("file:") >= 0)
                {
                    continue;
                }
                /*
                * 获取图片的Url
                */
                regex = new Regex("src=\"[^ \f\n\r\t\v][^\"]*\"", RegexOptions.IgnoreCase);
                int a = regex.Match(img).ToString().IndexOf('"') + 1;
                int b = regex.Match(img).ToString().LastIndexOf('"');
                if (b - a <= 0 || a == -1)
                {
                    continue;
                }
                string src = regex.Match(img).ToString().Substring(a, b - a);
                try
                {
                    /*
                     *生成图片缩略图
                     */
                    src = GetImageUrl(FilePath, src);
                    if (File.Exists(HttpContext.Current.Server.MapPath(src)))
                    {
                        int index = src.LastIndexOf(".");
                        string fileType = src.Substring(index).ToLower();
                        string filePath = "/UploadFiles/Associate/";
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath(filePath)))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(filePath));
                        }
                        int start = src.LastIndexOf('/');
                        int end = src.LastIndexOf('.');
                        //原图名称
                        string OriginalImgName = src.Substring(start + 1, end - start - 1);
                        //小图名称
                        string ThumbImgName = OriginalImgName + "_" + width + "_" + height + "_small";
                        //如果存在此缩略图
                        if (File.Exists(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType)))
                        {
                            System.Drawing.Image ttt = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType));
                            //实际宽高比
                            double Filter2 = Convert.ToDouble(ttt.Width) / ttt.Height;
                            if (Filter2 + 0.5 < Filter || Filter2 - 0.5 > Filter)
                            {
                                if (ttt != null) { ttt.Dispose(); }
                                continue;
                            }
                            if (ttt.Width == width)
                            {
                                ThumbImg = filePath + ThumbImgName + fileType;
                                if (ttt != null) { ttt.Dispose(); }
                                break;
                            }
                            if (ttt != null) { ttt.Dispose(); }
                        }
                        oImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(src));
                        int owidth = oImage.Width; //原图宽度
                        int oheight = oImage.Height; //原图高度
                                                     //实际宽高比
                        double Filter3 = Convert.ToDouble(oImage.Width) / oImage.Height;
                        if (Filter3 + 0.5 < Filter || Filter3 - 0.5 > Filter)
                        {
                            if (oImage != null) oImage.Dispose();
                            continue;
                        }
                        int width2 = width;
                        int height2 = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(width2) / Convert.ToDouble(owidth)));//等比设定高度
                                                                                                                                         //生成缩略原图 （大图）
                        sImage = new Bitmap(width2, height2);
                        sg = Graphics.FromImage(sImage);
                        sg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                        sg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                        sg.Clear(Color.Transparent); //清空画布并以透明背景色填充
                        sg.DrawImage(oImage, new Rectangle(0, 0, width2, height2), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);

                        switch (fileType)
                        {
                            case ".pjpeg":
                            case ".jpeg":
                            case ".jpg":
                                {
                                    sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    break;
                                }
                            case ".bmp":
                                {
                                    sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Bmp);
                                    break;
                                }
                            case ".gif":
                                {
                                    sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Gif);
                                    break;
                                }
                            case ".png":
                                {
                                    sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Png);
                                    break;
                                }
                        }
                        ThumbImg = filePath + ThumbImgName + fileType;
                    }
                    else
                    {
                        continue;
                    }
                    break;
                }
                catch (Exception)
                {
                    // LogHelper.AddErrorLog(ex);
                    result = false;
                }
                finally
                {
                    //释放资源
                    if (sg != null)
                        sg.Dispose();
                    if (sImage != null)
                        sImage.Dispose();
                    if (oImage != null)
                        oImage.Dispose();
                }
                if (!result)
                {
                    continue;
                }
            }
            return ThumbImg;
        }

        /// <summary>
        /// 返回内容区第一张图片的缩略图
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static string GetThumbImg(string Content, int width, int height)
        {
            string ThumbImg = string.Empty;
            string reg = "<img[ \f\n\r\t\v]*[^>]*/?>";
            Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
            MatchCollection array = regex.Matches(Content);
            for (int i = 0; i < array.Count; i++)
            {
                string img = array[i].Value;
                if (img.IndexOf("http://") >= 0 || img.IndexOf("https://") >= 0 || img.IndexOf("ftp:") >= 0 || img.IndexOf("file:") >= 0)
                {
                    continue;
                }
                /*
                * 获取图片的Url
                */
                regex = new Regex("src=\"[^ \f\n\r\t\v][^\"]*\"", RegexOptions.IgnoreCase);
                int a = regex.Match(img).ToString().IndexOf('"') + 1;
                int b = regex.Match(img).ToString().LastIndexOf('"');
                string src = regex.Match(img).ToString().Substring(a, b - a);

                /*
                 *生成图片缩略图
                 */
                if (File.Exists(HttpContext.Current.Server.MapPath(src)))
                {
                    int index = src.LastIndexOf(".");
                    string fileType = src.Substring(index).ToLower();
                    int index2 = src.LastIndexOf("/");
                    string filePath = src.Substring(0, index2 + 1);

                    int start = src.LastIndexOf('/');
                    int end = src.LastIndexOf('.');
                    //原图名称
                    string OriginalImgName = src.Substring(start + 1, end - start - 1);
                    //小图名称
                    string ThumbImgName = OriginalImgName + "_" + width + "_" + height + "_small";
                    //如果存在此缩略图
                    if (File.Exists(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType)))
                    {
                        ThumbImg = filePath + ThumbImgName + fileType;
                    }
                    else
                    {
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath(filePath)))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(filePath));
                        }
                        System.Drawing.Image oImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(src));
                        int owidth = oImage.Width; //原图宽度
                        int oheight = oImage.Height; //原图高度
                        int width2 = width;
                        int height2 = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(width2) / Convert.ToDouble(owidth)));//等比设定高度
                                                                                                                                         //生成缩略原图 （大图）
                        Bitmap sImage = new Bitmap(width2, height2);
                        Graphics sg = Graphics.FromImage(sImage);
                        sg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                        sg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                        sg.Clear(Color.Transparent); //清空画布并以透明背景色填充
                        sg.DrawImage(oImage, new Rectangle(0, 0, width2, height2), new Rectangle(0, 0, owidth, oheight), GraphicsUnit.Pixel);
                        try
                        {
                            switch (fileType)
                            {
                                case ".pjpeg":
                                case ".jpeg":
                                case ".jpg":
                                    {
                                        sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        break;
                                    }
                                case ".bmp":
                                    {
                                        sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Bmp);
                                        break;
                                    }
                                case ".gif":
                                    {
                                        sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Gif);
                                        break;
                                    }
                                case ".png":
                                    {
                                        sImage.Save(HttpContext.Current.Server.MapPath(filePath + ThumbImgName + fileType), System.Drawing.Imaging.ImageFormat.Png);
                                        break;
                                    }
                            }
                            ThumbImg = filePath + ThumbImgName + fileType;
                        }
                        catch (Exception)
                        {
                            //LogHelper.AddErrorLog(ex);
                        }
                        finally
                        {
                            //释放资源
                            sg.Dispose();
                            sImage.Dispose();
                            oImage.Dispose();
                        }
                    }
                }
                else
                {
                    continue;
                }
                break;
            }
            return ThumbImg;
        }

        /// <summary>
        /// 将存在../和没有/的图片目录更改为相对于根目录的图片路径
        /// </summary>
        /// <param name="FilePath">当前html页面所在完整目录</param>
        /// <param name="ImageUrl">要更改的图片路径</param>
        /// <returns>更改后的图片目录</returns>
        public static string GetImageUrl(string FilePath, string ImageUrl)
        {
            string url = string.Empty;
            //根目录
            string basepath = HttpContext.Current.Server.MapPath("/");
            //当前目录
            string _path = string.Empty;
            if (FilePath.IndexOf(basepath) != -1)
            {
                _path = FilePath.Substring(basepath.Length - 1).Replace("\\", "/");
            }
            switch (ImageUrl.IndexOf("/"))
            {
                case 0:
                    url = ImageUrl;
                    break;

                case -1:
                    if (_path.LastIndexOf("/") != _path.Length - 1)
                    {
                        _path = _path + "/";
                    }
                    url = _path + ImageUrl;
                    break;

                default:
                    Regex reg = new Regex("\\.\\./");
                    int count = reg.Matches(ImageUrl).Count;
                    if (count > 0)
                    {
                        int index = ImageUrl.LastIndexOf("../");
                        if (FilePath.LastIndexOf("\\") != FilePath.Length - 1)
                        {
                            FilePath = FilePath + "\\";
                        }
                        DirectoryInfo dir = new DirectoryInfo(FilePath);
                        string fullname = dir.FullName;
                        switch (count)
                        {
                            case 1:
                                if (fullname.LastIndexOf("\\") != fullname.Length - 1)
                                {
                                    fullname = fullname + "\\";
                                }
                                if (fullname != basepath)
                                {
                                    _path = dir.Parent.FullName;
                                }
                                if (_path.LastIndexOf("\\") != _path.Length - 1)
                                {
                                    _path = _path + "\\";
                                }
                                break;

                            case 2:
                                if (fullname.LastIndexOf("\\") != fullname.Length - 1)
                                {
                                    fullname = fullname + "\\";
                                }
                                if (fullname != basepath)
                                {
                                    _path = dir.Parent.FullName;
                                    if (_path.LastIndexOf("\\") != _path.Length - 1)
                                    {
                                        _path = _path + "\\";
                                    }
                                    if (_path != basepath)
                                    {
                                        _path = dir.Parent.Parent.FullName;
                                    }
                                    if (_path.LastIndexOf("\\") != _path.Length - 1)
                                    {
                                        _path = _path + "\\";
                                    }
                                }
                                break;

                            case 3:
                                if (fullname.LastIndexOf("\\") != fullname.Length - 1)
                                {
                                    fullname = fullname + "\\";
                                }
                                if (fullname != basepath)
                                {
                                    _path = dir.Parent.FullName;
                                    if (_path.LastIndexOf("\\") != _path.Length - 1)
                                    {
                                        _path = _path + "\\";
                                    }
                                    if (_path != basepath)
                                    {
                                        _path = dir.Parent.Parent.FullName;
                                        if (_path.LastIndexOf("\\") != _path.Length - 1)
                                        {
                                            _path = _path + "\\";
                                        }
                                        if (_path != basepath)
                                        {
                                            _path = dir.Parent.Parent.Parent.FullName;
                                        }
                                        if (_path.LastIndexOf("\\") != _path.Length - 1)
                                        {
                                            _path = _path + "\\";
                                        }
                                    }
                                }
                                break;

                            case 4:
                                if (fullname.LastIndexOf("\\") != fullname.Length - 1)
                                {
                                    fullname = fullname + "\\";
                                }
                                if (fullname != basepath)
                                {
                                    _path = dir.Parent.FullName;
                                    if (_path.LastIndexOf("\\") != _path.Length - 1)
                                    {
                                        _path = _path + "\\";
                                    }
                                    if (_path != basepath)
                                    {
                                        _path = dir.Parent.Parent.FullName;
                                        if (_path.LastIndexOf("\\") != _path.Length - 1)
                                        {
                                            _path = _path + "\\";
                                        }
                                        if (_path != basepath)
                                        {
                                            _path = dir.Parent.Parent.Parent.FullName;
                                            if (_path.LastIndexOf("\\") != _path.Length - 1)
                                            {
                                                _path = _path + "\\";
                                            }
                                            if (_path != basepath)
                                            {
                                                _path = dir.Parent.Parent.Parent.Parent.FullName;
                                            }
                                            if (_path.LastIndexOf("\\") != _path.Length - 1)
                                            {
                                                _path = _path + "\\";
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                        if (_path == basepath)
                        {
                            _path = "/";
                        }
                        else if (_path.Length > basepath.Length)
                        {
                            _path = _path.Substring(basepath.Length - 1).Replace("\\", "/");
                        }
                        url = _path + ImageUrl.Substring(index + 3);
                    }
                    else
                    {
                        if (_path.LastIndexOf("/") != _path.Length - 1)
                        {
                            _path = _path + "/";
                        }
                        url = _path + ImageUrl;
                    }
                    break;
            }
            return url;
        }

        /// <summary>
        /// 根据设定的宽生成一张缩略图(支持图片类型：jpg/bmp/png)，缩略图格式：TyyMMdd999999999.jpg/bmp/png，原图格式:yyMMdd999999999.jpg/bmp/png
        /// </summary>
        /// <param name="file">FileUpload file</param>
        /// <param name="thumbSavePath">缩略图保存路径</param>
        /// <param name="width">缩略图宽</param>
        /// <param name="imageName">返回缩略图名称</param>
        /// <param name="message">返回错误信息</param>
        /// <param name="isSaveOriginal">是否保存原图</param>
        /// <param name="origiSavePath">原图保存路径</param>
        /// <returns></returns>
        public static bool UploadImage(FileUpload file, string thumbSavePath, int width,
            out string imageName, out string message, bool isSaveOriginal, string origiSavePath, string Prefix)
        {
            imageName = string.Empty;
            message = string.Empty;
            bool Result = true;
            Stream oStream = null;
            System.Drawing.Image oImage = null;
            Bitmap tImage = null;
            Graphics g = null;
            string fileName = file.FileName.ToLower();
            int i = fileName.LastIndexOf(".");
            string fileType = fileName.Substring(i).ToLower();
            if (!(fileType == ".bmp" || fileType == ".jpeg" || fileType == ".jpg" || fileType == ".png"))
            {
                message = "请上传jpg/bmp/png格式的图片!";
                return false;
            }
            //生成6位随机数字
            Random r = new Random();
            string StringRandom = r.Next(999999999).ToString();
            //格式化日期作为文件名
            string StringTime = DateTime.Now.ToString("yyMMdd");
            imageName = StringTime + StringRandom + fileType;
            try
            {
                //保存原图
                if (isSaveOriginal)
                {
                    origiSavePath = HttpContext.Current.Server.MapPath(origiSavePath);
                    if (!Directory.Exists(origiSavePath))
                    {
                        Directory.CreateDirectory(origiSavePath);
                    }
                    file.SaveAs(origiSavePath + imageName);
                }
                //保存缩略图
                imageName = Prefix + imageName;
                thumbSavePath = HttpContext.Current.Server.MapPath(thumbSavePath);
                if (!Directory.Exists(thumbSavePath))
                {
                    Directory.CreateDirectory(thumbSavePath);
                }

                //生成原图
                oStream = file.PostedFile.InputStream;
                oImage = System.Drawing.Image.FromStream(oStream);
                int OWidth = oImage.Width; //原图宽度
                int OHeight = oImage.Height; //原图高度
                int height = 0;//定义缩略图高度
                if (OWidth > width)//原图尺寸大于小缩略图尺寸
                {
                    height = (int)Math.Floor(Convert.ToDouble(OHeight) * (Convert.ToDouble(width) / Convert.ToDouble(OWidth)));//等比设定高度
                    tImage = new Bitmap(width, height);
                    g = Graphics.FromImage(tImage);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; //设置高质量插值法
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                    g.Clear(Color.Transparent); //清空画布并以透明背景色填充
                    g.DrawImage(oImage, new Rectangle(0, 0, width, height), new Rectangle(0, 0, OWidth, OHeight), GraphicsUnit.Pixel);
                    //开始保存图片至服务器
                    switch (fileType)
                    {
                        case ".pjpeg":
                        case ".jpeg":
                        case ".jpg":
                            {
                                tImage.Save(thumbSavePath + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;
                            }
                        case ".png":
                            {
                                tImage.Save(thumbSavePath + imageName, System.Drawing.Imaging.ImageFormat.Png);
                                break;
                            }
                        case ".bmp":
                            {
                                tImage.Save(thumbSavePath + imageName, System.Drawing.Imaging.ImageFormat.Bmp);
                                break;
                            }
                    }
                }
                else
                {
                    file.SaveAs(thumbSavePath + imageName);
                }
                Result = true;
            }
            catch (Exception ex)
            {
                //LogHelper.AddErrorLog(ex);
                message = "处理图片出错，错误原因：" + ex.Message + "。详细错误信息请查看错误日志。";
                Result = false;
            }
            finally
            {
                //释放资源
                if (oImage != null) oImage.Dispose();
                if (g != null) g.Dispose();
                if (tImage != null) tImage.Dispose();
            }
            return Result;
        }

        /// <summary>
        /// 上传原图
        /// </summary>
        /// <param name="file"></param>
        /// <param name="SavePath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool UploadOriginalImage(FileUpload file, string SavePath, out string message, out string filename)
        {
            message = string.Empty;
            filename = string.Empty;
            bool Result = false;
            string fileName = file.FileName.ToLower();
            int i = fileName.LastIndexOf(".");
            string fileType = fileName.Substring(i).ToLower();
            if (!(fileType == ".jpeg" || fileType == ".jpg"))
            {
                message = "请上传jpg格式的图片!";
                return false;
            }
            //生成6位随机数字
            Random r = new Random();
            string StringRandom = r.Next(999999999).ToString();
            //格式化日期作为文件名
            string StringTime = DateTime.Now.ToString("yyMMdd");
            filename = StringTime + StringRandom + fileType;
            try
            {
                string origiSavePath = HttpContext.Current.Server.MapPath(SavePath);
                if (!Directory.Exists(origiSavePath))
                {
                    Directory.CreateDirectory(origiSavePath);
                }
                file.SaveAs(origiSavePath + filename);
                Result = true;
            }
            catch (Exception ex)
            {
                // LogHelper.AddErrorLog(ex);
                message = "处理图片出错，错误原因：" + ex.Message + "。详细错误信息请查看错误日志。";
                Result = false;
            }
            return Result;
        }
    }
}