using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.AdvancedAPIs.RedPack.inter
{
    public interface ISendCert
    {
        Task<string> PostAsync(string data);

        string Post(string data);

        string PostByHttpWebRequest(string data);
    }
}
