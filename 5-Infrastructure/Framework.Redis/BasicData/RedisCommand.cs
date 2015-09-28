using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.BasicData
{
    public class RedisCommand
    {
        /// <summary>
        /// 换行符
        /// </summary>
        public const string NEW_LINE = "\r\n";

        /// <summary>
        /// 成功状态
        /// </summary>
        public const string STATUS_SUCCESS = "+OK";

        /// <summary>
        /// 整数回复
        /// </summary>
        public const string REPLAY_INTEGER = ":";
    }
}
