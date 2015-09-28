using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Caching.Helpers
{
    public static class ReflectExtensions
    {
        public static void CreateDelegate<TDelegate>(this MethodInfo method, out TDelegate result)
        {
            result = (TDelegate)(object)Delegate.CreateDelegate(typeof(TDelegate), method);
        }
    }
}
