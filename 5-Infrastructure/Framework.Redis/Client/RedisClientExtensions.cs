using Framework.Redis.Command.SetCommand;
using Framework.Redis.Command.StringCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Client
{
    public partial class RedisClient
    {
        public string Get(string key)
        {
            return new Get().Send(new Parameters.StringParameter.GetParameter() { Key = key });
        }

        public void Set(string key, string value)
        {
            new Set().SendNonReturn(new Parameters.StringParameter.SetParameter() { Key = key, Value = value });
        }

        public string HGet(string key, string field)
        {
            return new HGet().Send(new Parameters.SetParameter.HGetParameter() { Key = key, Field = field });
        }

        public T HGet<T>(string key, string field)
            where T : class
        {
            try
            {
                var json = new HGet().Send(new Parameters.SetParameter.HGetParameter() { Key = key, Field = field });
                return Framework.Common.SerializeOperation.JsonParser.DeserializeFromJson<T>(json);
            }
            catch
            {
                return null;
            }
        }

        public void HSet(string key, string field, string value)
        {
            new HSet().SendNonReturn(new Parameters.SetParameter.HSetParameter() { Key = key, Field = field, Value = value });
        }

        public void HSet(string key, string field, object value)
        {
            try
            {
                var json = Framework.Common.SerializeOperation.JsonParser.SerializeToJson(value);
                new HSet().SendNonReturn(new Parameters.SetParameter.HSetParameter() { Key = key, Field = field, Value = json });
            }
            catch
            { }
        }
    }
}
