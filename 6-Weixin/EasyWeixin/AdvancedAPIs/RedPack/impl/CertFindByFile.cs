using EasyWeixin.AdvancedAPIs.RedPack.inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EasyWeixin.AdvancedAPIs.RedPack.impl
{
    public class CertFindByFile : ICertFinder
    {
        readonly string _path;
        readonly string _password;
        public CertFindByFile(string path, string password)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path isNullOrEmpty");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password isNullOrEmpty");

            _path = path;
            _password = password;
        }
        public X509Certificate2 Find()
        {
            X509Certificate2 cert = new X509Certificate2(_path, _password);
            return cert;
        }
    }
}
