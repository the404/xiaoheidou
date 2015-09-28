using Framework.Redis.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Parameters.StringParameter
{
    public class GetParameter : CommandParameterBase
    {
        public string Key { get; set; }
    }
}
