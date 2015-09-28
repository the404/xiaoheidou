using Framework.Redis.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis
{
    public class RedisSingleton
    {
        #region 单例

        private static RedisSingleton _redisSingleton = null;
        private static object _locker = new object();

        public static RedisSingleton GetInstance
        {
            get
            {
                if (_redisSingleton == null)
                {
                    lock (_locker)
                    {
                        if (_redisSingleton == null)
                        {
                            _redisSingleton = new RedisSingleton();
                        }
                    }
                }

                return _redisSingleton;
            }
        }

        #endregion

        public RedisClient Client { get; set; }
    }
}
