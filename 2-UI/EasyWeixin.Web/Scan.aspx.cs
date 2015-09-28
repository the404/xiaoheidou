using EasyWeixin.Entities.JsonResult;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EasyWeixin.Web
{
    public partial class Scan : System.Web.UI.Page
    {
        static string appid = "wx00f1a2e4c8da9fff";
        static string appsecert = "d0cecf2fa0b11a67077a4cbeb4c127ab";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                WxConfig = EasyWeixin.CommonAPIs.CommonApi.GetWxConfigResult(appid, appsecert, Request.Url.ToString());
                QRCodeEncoder encoder = new QRCodeEncoder();
                Bitmap bt = encoder.Encode(Request.Url.ToString());


                var md5 = new MD5CryptoServiceProvider();
                var start = Encoding.Default.GetBytes(Request.Url.ToString());
                byte[] output = md5.ComputeHash(start);
                var result = BitConverter.ToString(output).Replace("-", "");

                var direName = "~/tmpQrImg/";
                var filename = direName + result + ".png";
                var filepath = Server.MapPath(filename);

                if (!Directory.Exists(direName))
                {
                    Directory.CreateDirectory(direName);
                }

                if (!File.Exists(filename))
                {
                    bt.Save(filename, ImageFormat.Png);
                }
                ImgSrc = filename;
            }
        }

        public WxConfigResult WxConfig { get; set; }
        public string ImgSrc { get; set; }
    }
}