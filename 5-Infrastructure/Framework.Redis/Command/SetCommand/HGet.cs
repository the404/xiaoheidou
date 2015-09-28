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
    public class HGet : ICommand<HGetParameter, string>
    {
        private const string HGET_TEMPLATE = "HGET {0} {1}";

        #region ICommand<CommandParameterBase> 成员

        public string Send(HGetParameter commandParameter = null)
        {
            var result = RedisSingleton.GetInstance.Client.Send(string.Format(HGET_TEMPLATE, commandParameter.Key, commandParameter.Field));
            var base64String = result.Split(new string[] { RedisCommand.NEW_LINE }, StringSplitOptions.None)[1];

            return commandParameter.GetValueFromBase64String(base64String);
        }

        public void SendNonReturn(HGetParameter commandParameter = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
