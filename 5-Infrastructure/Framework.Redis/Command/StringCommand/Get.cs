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
    public class Get : ICommand<GetParameter, string>
    {
        private const string GET_TEMPLATE = "GET {0}";

        #region ICommand<CommandParameterBase> 成员

        public string Send(GetParameter commandParameter = null)
        {
            var result = RedisSingleton.GetInstance.Client.Send(string.Format(GET_TEMPLATE, commandParameter.Key));
            var base64String = result.Split(new string[] { RedisCommand.NEW_LINE }, StringSplitOptions.None)[1];

            return commandParameter.GetValueFromBase64String(base64String);
        }

        public void SendNonReturn(GetParameter commandParameter = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
