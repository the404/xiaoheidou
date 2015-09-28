using EasyWeixin.AdvancedAPIs.RedPack.inter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.AdvancedAPIs.RedPack.impl
{
    public class SendCert : ISendCert
    {
        static readonly string _WeChatRedPacketApiEndpoint = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

        ICertFinder _certFinder;

        public SendCert(ICertFinder finder)
        {
            if (finder == null)
                throw new ArgumentNullException("finder");

            _certFinder = finder;
        }
        public async Task<string> PostAsync(string data)
        {
            var cert = _certFinder.Find();

            WebRequestHandler certHandler = new WebRequestHandler();
            certHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            certHandler.UseDefaultCredentials = false;

            certHandler.ClientCertificates.Add(cert);

            using (var client = new HttpClient(certHandler, true))
            {
                HttpResponseMessage response = await client.PostAsync(_WeChatRedPacketApiEndpoint,
                    new StringContent(data, Encoding.UTF8, "application/xml"));

                var responseData = await response.Content.ReadAsByteArrayAsync();
                var result = Encoding.UTF8.GetString(responseData);
                return result;
            }
        }

        public string PostByHttpWebRequest(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            var cert = _certFinder.Find();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_WeChatRedPacketApiEndpoint);
            request.Method = "POST";
            request.UseDefaultCredentials = false;
            request.ClientCertificates.Add(cert);

            //这两个参数其实很关键,一个表示发送数据的格式,这样接受请求服务器的服务器才能知道怎么去解析
            request.ContentType = "application/xml";
            request.ContentLength = buffer.Length;//如果长度少于的话,设置GetRequestStream时将会抛出异常

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);//将数据写入流中                
            }

            using (Stream s = request.GetResponse().GetResponseStream())
            {
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                return sr.ReadToEnd();
            }
        }


        public string Post(string data)
        {
            var cert = _certFinder.Find();

            WebRequestHandler certHandler = new WebRequestHandler();
            certHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            certHandler.UseDefaultCredentials = false;

            certHandler.ClientCertificates.Add(cert);

            using (var client = new HttpClient(certHandler, true))
            {
                HttpResponseMessage response = client.PostAsync(_WeChatRedPacketApiEndpoint,
                    new StringContent(data, Encoding.UTF8, "application/xml")).Result;

                var responseData = response.Content.ReadAsByteArrayAsync().Result;
                var result = Encoding.UTF8.GetString(responseData);
                return result;
            }
        }
    }
}
