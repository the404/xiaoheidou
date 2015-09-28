using EasyWeixin.AdvancedAPIs.RedPack.impl;
using EasyWeixin.AdvancedAPIs.RedPack.inter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EasyWeixin.AdvancedAPIs.RedPack
{
    public class RedPacketService : IRedPacketService
    {
        readonly string _payKey;
        readonly string _mchId;
        readonly ISendCert _sendCert;

        static RedPacketService() //静态构造函数
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            return false;
        }
        public RedPacketService(string mchId, string payKey, ISendCert sendCert)
        {
            if (mchId == null)
                throw new ArgumentNullException("merchantId");
            if (payKey == null)
                throw new ArgumentNullException("payKey");
            if (sendCert == null)
                throw new ArgumentNullException("sendCert");

            _mchId = mchId;
            _payKey = payKey;
            _sendCert = sendCert;
        }

        public async Task<RedPacketSentResult> SendAsync(RedPacket redPacket)
        {
            if (redPacket == null)
                throw new ArgumentNullException("redPacket");
            if (!redPacket.IsValid())
                throw new ArgumentException("redPacket");

            var amount = Amounter.GetAmount(redPacket.Amount);
            var amountString = amount.ToString();

            var sendData = new SortedList<string, string>(StringComparer.Ordinal);
            sendData.Add("nonce_str", Guid.NewGuid().ToString("N"));
            sendData.Add("mch_billno", redPacket.BillNumber);
            sendData.Add("mch_id", _mchId);
            sendData.Add("wxappid", redPacket.AppId);
            sendData.Add("nick_name", redPacket.SendName); //简单起见使用send_name。
            sendData.Add("send_name", redPacket.SendName);
            sendData.Add("re_openid", redPacket.OpenId);
            sendData.Add("total_amount", amountString);
            sendData.Add("min_value", amountString);
            sendData.Add("max_value", amountString);
            sendData.Add("total_num", "1");
            sendData.Add("wishing", redPacket.Wishing);
            sendData.Add("client_ip", redPacket.IpAddress);
            sendData.Add("act_name", redPacket.ActName);
            sendData.Add("remark", redPacket.Remark);
            var xml = DictionaryToXml(sendData);

            var result = await _sendCert.PostAsync(xml);
            var result2 = Parse(result);
            result2.Amount = amount;
            result2.BillNumber = redPacket.BillNumber;
            return result2;
        }

        public RedPacketSentResult Send(RedPacket redPacket)
        {
            if (redPacket == null)
                throw new ArgumentNullException("redPacket");
            if (!redPacket.IsValid())
                throw new ArgumentException("redPacket");

            var amount = Amounter.GetAmount(redPacket.Amount);
            var amountString = amount.ToString();

            var sendData = new SortedList<string, string>(StringComparer.Ordinal);
            sendData.Add("nonce_str", Guid.NewGuid().ToString("N"));
            sendData.Add("mch_billno", redPacket.BillNumber);
            sendData.Add("mch_id", _mchId);
            sendData.Add("wxappid", redPacket.AppId);
            sendData.Add("nick_name", redPacket.SendName); //简单起见使用send_name。
            sendData.Add("send_name", redPacket.SendName);
            sendData.Add("re_openid", redPacket.OpenId);
            sendData.Add("total_amount", amountString);
            sendData.Add("min_value", amountString);
            sendData.Add("max_value", amountString);
            sendData.Add("total_num", "1");
            sendData.Add("wishing", redPacket.Wishing);
            sendData.Add("client_ip", redPacket.IpAddress);
            sendData.Add("act_name", redPacket.ActName);
            sendData.Add("remark", redPacket.Remark);
            var xml = DictionaryToXml(sendData);

            var result = _sendCert.Post(xml);
            var result2 = Parse(result);
            result2.Amount = amount;
            result2.BillNumber = redPacket.BillNumber;
            return result2;
        }

        public RedPacketSentResult SendByHttpWebRequest(RedPacket redPacket)
        {
            if (redPacket == null)
                throw new ArgumentNullException("redPacket");
            if (!redPacket.IsValid())
                throw new ArgumentException("redPacket");

            var amount = Amounter.GetAmount(redPacket.Amount);
            var amountString = amount.ToString();

            var sendData = new SortedList<string, string>(StringComparer.Ordinal);
            sendData.Add("nonce_str", Guid.NewGuid().ToString("N"));
            sendData.Add("mch_billno", redPacket.BillNumber);
            sendData.Add("mch_id", _mchId);
            sendData.Add("wxappid", redPacket.AppId);
            sendData.Add("nick_name", redPacket.SendName); //简单起见使用send_name。
            sendData.Add("send_name", redPacket.SendName);
            sendData.Add("re_openid", redPacket.OpenId);
            sendData.Add("total_amount", amountString);
            sendData.Add("min_value", amountString);
            sendData.Add("max_value", amountString);
            sendData.Add("total_num", "1");
            sendData.Add("wishing", redPacket.Wishing);
            sendData.Add("client_ip", redPacket.IpAddress);
            sendData.Add("act_name", redPacket.ActName);
            sendData.Add("remark", redPacket.Remark);
            var xml = DictionaryToXml(sendData);

            var result = _sendCert.PostByHttpWebRequest(xml);
            var result2 = Parse(result);
            result2.Amount = amount;
            result2.BillNumber = redPacket.BillNumber;
            return result2;
        }

        public RedPacketSentResult Parse(string xmlStr)
        {
            var result = new RedPacketSentResult { Succeeded = false, Response = xmlStr };
            try
            {
                var error = RedPacketSentError.None;
                var xml = XDocument.Parse(xmlStr).Root;
                var returnCode = xml.Element("return_code").Value;

                try
                {
                    var err_code = xml.Element("err_code").Value;
                    var balanceNotEnough = string.Equals(err_code, "NOTENOUGH", StringComparison.OrdinalIgnoreCase);
                    error = balanceNotEnough ? RedPacketSentError.BalanceNotEnough : RedPacketSentError.Other;
                }
                catch
                {
                    //不需要任何的处理,只是表示没有err_code这个节点而已
                }

                if (!string.Equals(returnCode, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                {
                    result.Error = error;
                }
                else
                {

                    var resultCode = xml.Element("result_code").Value;
                    if (!string.Equals(resultCode, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Error = error;
                    }
                    else
                    {
                        result.Succeeded = true;
                    }
                }

            }
            catch (Exception)
            {
                //Logger.WriteError("解析微信返回结果时发生错误", ex);
                result.Error = RedPacketSentError.InternalError;
            }

            return result;
        }

        string Sign(IDictionary<string, string> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in dict)
            {
                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value))
                    continue;

                sb.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            sb.Append("key=" + _payKey);
            var bytesToHash = Encoding.UTF8.GetBytes(sb.ToString()); //注意，必须是UTF-8。
            var hashResult = ComputeMD5Hash(bytesToHash);
            var hash = BytesToString(hashResult, false);
            return hash;
        }

        static byte[] ComputeMD5Hash(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var result = md5.ComputeHash(input);
                return result;
            }
        }

        public static string BytesToString(byte[] input, bool lowercase = true)
        {
            if (input == null || input.Length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder(input.Length * 2);
            for (var i = 0; i < input.Length; i++)
            {
                sb.AppendFormat(lowercase ? "{0:x2}" : "{0:X2}", input[i]);
            }
            return sb.ToString();
        }

        string DictionaryToXml(IDictionary<string, string> dict)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<xml>");
            foreach (var item in dict)
            {
                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value))
                    continue;

                sb.AppendFormat("<{0}>{1}</{0}>", item.Key, item.Value);
            }
            var sign = Sign(dict);
            sb.AppendFormat("<sign>{0}</sign>", sign);
            sb.AppendLine("</xml>");
            return sb.ToString();
        }

        Task<RedPacketSentResult> IRedPacketService.SendAsync(RedPacket request)
        {
            throw new NotImplementedException();
        }
    }
}
