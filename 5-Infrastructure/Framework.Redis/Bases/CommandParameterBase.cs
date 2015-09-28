using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Bases
{

    public class CommandParameterBase
    {
        public string Value { get; set; }

        public string GetValueToBase64String()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(this.Value);
            return Convert.ToBase64String(bytes);
        }

        public string GetValueFromBase64String(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
