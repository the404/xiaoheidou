using Framework.Redis.BasicData;
using Framework.Redis.Interfaces;
using Framework.Redis.Parameters.StringParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Command.StringCommand
{
    public class Set : ICommand<SetParameter, string>
    {
        private const string SET_TEMPLATE = "SET {0} {1}";

        #region ICommand<SetParameter,string> 成员

        public string Send(SetParameter commandParameter = null)
        {
            throw new NotImplementedException();
        }

        public void SendNonReturn(SetParameter commandParameter = null)
        {
            var result = RedisSingleton.GetInstance.Client.Send(string.Format(SET_TEMPLATE, commandParameter.Key, commandParameter.GetValueToBase64String()));

            if (result != RedisCommand.STATUS_SUCCESS)
            {
                throw new Exception("redis error:" + result);
            }
        }

        #endregion
    }
}
