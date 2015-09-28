using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.SerializeOperation
{
    public class JsonParser
    {
        /// <summary>
        /// 序列化成json串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToJson(object obj)
        {
            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    json.WriteObject(ms, obj);
                    ms.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(ms);
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 反序列化json对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(string json) where T : class
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                T obj = serializer.ReadObject(ms) as T;
                return obj;
            }
        }
    }
}
