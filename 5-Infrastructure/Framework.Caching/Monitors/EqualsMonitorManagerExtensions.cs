using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Caching.Monitors
{
    public static partial class EqualsMonitorManager<TKey, TValue>
        where TValue : IEquatable<TValue>
    {
        private static class MonitorCaller<TCallerKey>
        {
            public static Action<TCallerKey, TValue> Add;

            public static Action<TCallerKey> Remove;

            public static Func<TCallerKey, Func<TValue, bool>, TValue> Get;

            public static Func<TCallerKey, TValue, bool> IsMonitoring;
        }

        #region Members

        private static Dictionary<string, List<TValue>> _dicStringMonitor = new Dictionary<string, List<TValue>>();

        #endregion

        static EqualsMonitorManager()
        {
            StringMonitorCallerInit();
        }

        private static void StringMonitorCallerInit()
        {
            MonitorCaller<string>.Add = (string key, TValue value) =>
            {
                if (!_dicStringMonitor.ContainsKey(key))
                {
                    _dicStringMonitor.Add(key, new List<TValue>());
                }

                _dicStringMonitor[key].Add(value);
            };

            MonitorCaller<string>.Remove = (string key) =>
            {
                if (_dicStringMonitor.ContainsKey(key))
                    _dicStringMonitor.Remove(key);
            };

            MonitorCaller<string>.Get = (string key, Func<TValue, bool> predicate) =>
            {
                if (_dicStringMonitor.ContainsKey(key))
                    return _dicStringMonitor[key].FirstOrDefault(predicate);
                else
                    return default(TValue);
            };

            MonitorCaller<string>.IsMonitoring = (string key, TValue value) =>
            {
                if (!_dicStringMonitor.ContainsKey(key))
                {
                    return false;
                }

                return _dicStringMonitor[key].Exists(x => x.Equals(value));
            };
        }
    }
}
