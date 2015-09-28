using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Caching.Monitors
{
    public static partial class EqualsMonitorManager<TKey, TValue>
    {
        public static void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            MonitorCaller<TKey>.Add(key, value);
        }

        public static void Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            MonitorCaller<TKey>.Remove(key);
        }

        public static TValue Get(TKey key, Func<TValue, bool> predicate)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            return MonitorCaller<TKey>.Get(key, predicate);
        }

        public static bool IsMonitoring(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            return MonitorCaller<TKey>.IsMonitoring(key, value);
        }
    }
}
