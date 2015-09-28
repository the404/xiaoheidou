using EasyWeixin.AdvancedAPIs.RedPack.inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EasyWeixin.AdvancedAPIs.RedPack.impl
{
    /// <summary>
    /// 获取windows的证书管理器中的证书
    /// </summary>
    public class CertFinderByCertMgr : ICertFinder
    {
        string m_SubjectDistinguishedName;//主题名称

        public CertFinderByCertMgr(string subjectDistinguishedName)
        {
            if (string.IsNullOrEmpty(subjectDistinguishedName))
                throw new ArgumentException("string.IsNullOrEmpty");

            m_SubjectDistinguishedName = subjectDistinguishedName;
        }

        public System.Security.Cryptography.X509Certificates.X509Certificate2 Find()
        {
            var store = new System.Security.Cryptography.X509Certificates.X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, m_SubjectDistinguishedName, false);

            if (certs.Count == 0)
                throw new Exception("无法找到微信支付证书");

            return certs[0];
        }
    }
}
