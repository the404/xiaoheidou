using Framework.Redis.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Parameters.SetParameter
{
    public class HGetParameter : CommandParameterBase
    {
        public string Key { get; set; }

        public string Field { get; set; }
    }
}
