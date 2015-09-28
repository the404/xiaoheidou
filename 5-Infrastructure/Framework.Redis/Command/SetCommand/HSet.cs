using Framework.Redis.BasicData;
using Framework.Redis.Interfaces;
using Framework.Redis.Parameters.SetParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Command.SetCommand
{
    public class HSet : ICommand<HSetParameter, string>
    {
        private const string HSET_TEMPLATE = "HSET {0} {1} {2}";

        #region ICommand<HSetParameter,string> 成员

        public string Send(HSetParameter commandParameter = null)
        {
            throw new NotImplementedException();
        }

        public void SendNonReturn(HSetParameter commandParameter = null)
        {
            var result = RedisSingleton.GetInstance.Client.Send(string.Format(HSET_TEMPLATE, commandParameter.Key, commandParameter.Field, commandParameter.GetValueToBase64String()));

            if (!result.StartsWith(RedisCommand.REPLAY_INTEGER))
            {
                throw new Exception("redis error:" + result);
            }
        }

        #endregion
    }
}
