using System;
using System.Drawing;

namespace EasyWeixin.Helpers
{
    public class PicHelper
    {
        public static bool BuilderThumbnail(string sourcePath, string thumbnailPath, int targetWidth, int targetHeight, PicThumbnailType type)
        {
            var res = false;
            try
            {
                System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(sourcePath);
                if (sourceImage != null)
                {
                    System.Drawing.Size imgSize = new Size(sourceImage.Width, sourceImage.Height);
                    sourceImage.Dispose();
                    int newWidth = targetWidth;
                    int newHeight = targetHeight;
                    int cutX = 0;
                    int cutY = 0;
                    int cutWidth = imgSize.Width;
                    int cutHeight = imgSize.Height;

                    #region ready

                    switch (type)
                    {
                        case PicThumbnailType.DBL:
                            float temp = (float)cutWidth / (float)cutHeight;
                            temp = newWidth / temp;
                            newHeight = (int)temp;
                            break;

                        case PicThumbnailType.HW://指定高宽缩放（可能变形）
                            break;

                        case PicThumbnailType.W://指定宽，高按比例
                            newHeight = imgSize.Height * targetWidth / imgSize.Width;
                            break;

                        case PicThumbnailType.H://指定高，宽按比例
                            newWidth = imgSize.Width * targetHeight / imgSize.Height;
                            break;

                        case PicThumbnailType.CUT://指定高宽裁减（不变形）
                            if ((double)imgSize.Width / (double)imgSize.Height > (double)newWidth / (double)newHeight)
                            {
                                cutHeight = imgSize.Height;
                                cutWidth = imgSize.Height * newWidth / newHeight;
                                cutY = 0;
                                cutX = (imgSize.Width - cutWidth) / 2;
                            }
                            else
                            {
                                cutWidth = imgSize.Width;
                                cutHeight = imgSize.Width * targetHeight / newWidth;
                                cutX = 0;
                                cutY = (imgSize.Height - cutHeight) / 2;
                            }
                            break;

                        default:
                            break;
                    }

                    #endregion ready

                    res = BuilderThumbnailBase(sourcePath, thumbnailPath, newWidth, newHeight, cutX, cutY, cutWidth, cutHeight);
                }
            }
            catch (Exception)
            {
            }
            return res;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="sourcePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="targetWidth">缩略图宽度</param>
        /// <param name="targetHeight">缩略图高度</param>
        /// <param name="cutX"></param>
        /// <param name="cutY"></param>
        /// <param name="cutWidth"></param>
        /// <param name="cutHeight"></param>
        /// <returns></returns>
        private static bool BuilderThumbnailBase(string sourcePath, string thumbnailPath, int targetWidth, int targetHeight, int cutX, int cutY, int cutWidth, int cutHeight)
        {
            var res = false;
            System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(sourcePath);
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(targetWidth, targetHeight);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, targetWidth, targetHeight),//在指定位置并且按指定大小绘制原图片的指定部分
                new System.Drawing.Rectangle(cutX, cutY, cutWidth, cutHeight), System.Drawing.GraphicsUnit.Pixel);
            try
            {
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);  //以jpg格式保存缩略图
                res = true;
            }
            catch (System.Exception)
            {
            }
            finally
            {
                sourceImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return res;
        }

        /// <summary>
        /// 在图片上增加文字水印 系统默认方法 字体：Verdana，大小：20，颜色：Green
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="newPath">The new path.</param>
        /// <param name="letter">水印的文字</param>
        /// <param name="positionType">图片水印位置:UL，UM ， UR ， ML ， MM ， MR ， BL ， BM ， BR ，CUSTORM 为自定义</param>
        /// <returns></returns>
        public static bool TextWatermark(string sourcePath, string newPath, string letter, PicWaterMarkPosition positionType)
        {
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 20);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
            return TextWatermark(sourcePath, newPath, letter, f, b, 0, 0, positionType);
        }

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        /// <param name="letter">水印的文字</param>
        /// <param name="font">字体及大小</param>
        /// <param name="brush">画笔主要是颜色</param>
        /// <param name="txtPositionX">坐标X</param>
        /// <param name="txtPositionY">坐标Y</param>
        /// <param name="positionTpye">图片水印位置:UL，UM ， UR ， ML ， MM ， MR ， BL ， BM ， BR ，CUSTORM 为自定义</param>
        /// <returns></returns>
        public static bool TextWatermark(string sourcePath, string newPath, string letter,
            System.Drawing.Font font, System.Drawing.Brush brush, int txtPositionX, int txtPositionY,
            PicWaterMarkPosition positionTpye)
        {
            string addText = letter;
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(sourcePath);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                System.Drawing.Font f = font;
                System.Drawing.Brush b = brush;
                switch (positionTpye)
                {
                    case PicWaterMarkPosition.CUSTORM:
                        g.DrawString(addText, f, b, txtPositionX, txtPositionY);
                        break;

                    case PicWaterMarkPosition.UL:
                        g.DrawString(addText, f, b, 2, 2);
                        break;

                    case PicWaterMarkPosition.UM:
                        g.DrawString(addText, f, b, image.Width / 2 - (letter.Length * font.Size) / 4, 2);
                        break;

                    case PicWaterMarkPosition.UR:
                        g.DrawString(addText, f, b, image.Width - (float)letter.Length / 1.9f * font.Size, 2);
                        break;

                    case PicWaterMarkPosition.ML:
                        g.DrawString(addText, f, b, 2, image.Height / 2 - font.Height / 4);
                        break;

                    case PicWaterMarkPosition.MM:
                        g.DrawString(addText, f, b, image.Width / 2 - (letter.Length * font.Size) / 4, image.Height / 2 - font.Height / 4);
                        break;

                    case PicWaterMarkPosition.MR:
                        // x.r y.m
                        g.DrawString(addText, f, b, image.Width - (float)letter.Length / 1.9f * font.Size, image.Height / 2 - font.Height / 4);
                        break;

                    case PicWaterMarkPosition.BL:
                        //x.l y.b
                        g.DrawString(addText, f, b, 2, image.Height - font.Height / 1.3f);
                        break;

                    case PicWaterMarkPosition.BM:
                        // x.m y.b
                        g.DrawString(addText, f, b, image.Width / 2 - (letter.Length * font.Size) / 4, image.Height - font.Height / 1.3f);
                        break;

                    case PicWaterMarkPosition.BR:
                        //x.r y.b
                        g.DrawString(addText, f, b, image.Width - (float)letter.Length / 1.9f * font.Size, image.Height - font.Height / 1.3f);
                        break;

                    default:
                        break;
                }
                g.Dispose();
                try
                {
                    image.Save(newPath);
                    image.Dispose();
                    return true;
                }
                catch (Exception)
                {
                    image.Dispose();
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 在图片上生成图片水印 默认是右下角
        /// </summary>
        /// <param name="sourcePath">原服务器图片路径</param>
        /// <param name="targetPath">生成的带图片水印的图片路径</param>
        /// <param name="waterMarkFilePath">水印的图片路径</param>
        /// <returns></returns>
        public static bool PictureWatermark(string sourcePath, string targetPath, string waterMarkFilePath)
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(sourcePath);
                System.Drawing.Image markImage = System.Drawing.Image.FromFile(waterMarkFilePath);
                System.Drawing.Bitmap outPut = new System.Drawing.Bitmap(image);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outPut);

                g.DrawImage(markImage, new System.Drawing.Rectangle(image.Width - markImage.Width - 15,
                    image.Height - markImage.Height - 15, markImage.Width,
                    markImage.Height), 0, 0, markImage.Width, markImage.Height,
                    System.Drawing.GraphicsUnit.Pixel);

                g.Dispose();
                image.Save(targetPath);
                outPut.Dispose();
                markImage.Dispose();
                image.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="sourcePath">原图片路径</param>
        /// <param name="targetPath">另存为路径</param>
        /// <param name="waterMarkFilePath">水印图片路径</param>
        /// <param name="positionType">图片水印位置:UL，UM ， UR ， ML ， MM ， MR ， BL ， BM ， BR ，CUSTORM 为自定义.</param>
        /// <param name="quality">是否是高质量图片 取值范围0--100</param>
        /// <param name="watermarkTransparency">图片水印透明度 取值范围1--10 (10为不透明)</param>
        /// <returns></returns>
        public static bool PictureWatermark(string sourcePath, string targetPath, string waterMarkFilePath, PicWaterMarkPosition positionType, int quality, int watermarkTransparency)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(sourcePath);
            System.Drawing.Bitmap outPut = new System.Drawing.Bitmap(img);
            try
            {
                #region using

                using (Graphics g = Graphics.FromImage(outPut))
                {
                    //设置高质量插值法
                    //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    //设置高质量,低速度呈现平滑程度
                    //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    System.Drawing.Image markImg = new System.Drawing.Bitmap(waterMarkFilePath);
                    if (markImg.Height >= img.Height || markImg.Width >= img.Width)
                    {
                        return false;
                    }
                    System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
                    System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();
                    colorMap.OldColor = System.Drawing.Color.FromArgb(255, 0, 255, 0);
                    colorMap.NewColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                    System.Drawing.Imaging.ColorMap[] remapTable = { colorMap };
                    imageAttributes.SetRemapTable(remapTable, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                    float transparency = 0.5F;
                    if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                    {
                        transparency = (watermarkTransparency / 10.0F);
                    }
                    float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

                    System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);
                    imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                    int xpos = 0;
                    int ypos = 0;

                    #region position

                    switch (positionType)
                    {
                        case PicWaterMarkPosition.UL:
                            xpos = (int)(img.Width * (float).01);
                            ypos = (int)(img.Height * (float).01);
                            break;

                        case PicWaterMarkPosition.UM:
                            xpos = (int)((img.Width * (float).50) - (markImg.Width / 2));
                            ypos = (int)(img.Height * (float).01);
                            break;

                        case PicWaterMarkPosition.UR:
                            xpos = (int)((img.Width * (float).99) - (markImg.Width));
                            ypos = (int)(img.Height * (float).01);
                            break;

                        case PicWaterMarkPosition.ML:
                            xpos = (int)(img.Width * (float).01);
                            ypos = (int)((img.Height * (float).50) - (markImg.Height / 2));
                            break;

                        case PicWaterMarkPosition.MM:
                            xpos = (int)((img.Width * (float).50) - (markImg.Width / 2));
                            ypos = (int)((img.Height * (float).50) - (markImg.Height / 2));
                            break;

                        case PicWaterMarkPosition.MR:
                            xpos = (int)((img.Width * (float).99) - (markImg.Width));
                            ypos = (int)((img.Height * (float).50) - (markImg.Height / 2));
                            break;

                        case PicWaterMarkPosition.BL:
                            xpos = (int)(img.Width * (float).01);
                            ypos = (int)((img.Height * (float).99) - markImg.Height);
                            break;

                        case PicWaterMarkPosition.BM:
                            xpos = (int)((img.Width * (float).50) - (markImg.Width / 2));
                            ypos = (int)((img.Height * (float).99) - markImg.Height);
                            break;

                        case PicWaterMarkPosition.BR:
                            xpos = (int)((img.Width * (float).99) - (markImg.Width));
                            ypos = (int)((img.Height * (float).99) - markImg.Height);
                            break;
                    }

                    #endregion position

                    g.DrawImage(markImg, new System.Drawing.Rectangle(xpos, ypos, markImg.Width, markImg.Height), 0, 0, markImg.Width, markImg.Height, System.Drawing.GraphicsUnit.Pixel, imageAttributes);
                    System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                    System.Drawing.Imaging.ImageCodecInfo ici = null;
                    foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
                    {
                        if (codec.MimeType.IndexOf("jpeg") > -1)
                        {
                            ici = codec;
                        }
                    }
                    System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
                    long[] qualityParam = new long[1];
                    if (quality < 0 || quality > 100)
                    {
                        quality = 80;
                    }
                    qualityParam[0] = quality;
                    System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                    encoderParams.Param[0] = encoderParam;
                    if (ici != null)
                    {
                        outPut.Save(targetPath, ici, encoderParams);
                    }
                    else
                    {
                        outPut.Save(targetPath);
                    }
                    g.Dispose();
                    markImg.Dispose();
                    imageAttributes.Dispose();
                }

                #endregion using
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (outPut != null)
                {
                    outPut.Dispose();
                }
                if (img != null)
                {
                    img.Dispose();
                }
            }
            return true;
        }
    }

    #region Pic

    /// <summary>
    ///
    /// 这个是在Helper项目里面，类：IMGHelper.cs会用到的，
    /// UL:左上, UM：中上, UR：右上, ML：左中, MM：中中, MR：右中, BL：左下, BM：左中, BR：右下,CUSTORM：自定义
    ///
    /// </summary>
    public enum PicWaterMarkPosition
    {
        /// <summary>
        /// UL:左上, UM：中上, UR：右上, ML：左中, MM：中中, MR：右中, BL：左下, BM：左中, BR：右下,CUSTORM：自定义
        /// </summary>
        UL, UM, UR, ML, MM, MR, BL, BM, BR, CUSTORM
    }

    /// <summary>
    ///
    ///  这个是在Helper项目里面，类：IMGHelper.cs会用到的，
    ///  DBL：等比例, HW：按高宽, W：按宽, H：按高, CUT：剪切
    ///
    /// </summary>
    public enum PicThumbnailType
    {
        /// <summary>
        ///  DBL：等比例, HW：按高宽, W：按宽, H：按高, CUT：剪切
        /// </summary>
        DBL, HW, W, H, CUT
    }

    #endregion Pic
}