using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.EnumOperation
{
    /// <summary>
    /// 枚举描述属性反射类
    /// </summary>
    public static class EnumHelper
    {
        public static ExtensionAttribute GetEnumDescription(object enumSubitem)
        {
            string value = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(value);

            if (fieldinfo != null)
            {
                Object[] objs = fieldinfo.GetCustomAttributes(typeof(ExtensionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    return new ExtensionAttribute(value);
                }
                else
                {
                    return (ExtensionAttribute)objs[0];
                }
            }
            else
            {
                return new ExtensionAttribute();
            }
        }

        /// <summary>
        /// 获取枚举属性
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumSubitem">枚举子项</param>
        /// <returns>返回属性</returns>
        public static T GetEnumAttribute<T>(object enumSubitem) where T : Attribute, new()
        {
            string value = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(value);

            if (fieldinfo != null)
            {
                Object[] objs = fieldinfo.GetCustomAttributes(typeof(T), false);

                if (objs == null || objs.Length == 0)
                {
                    return new T();
                }
                else
                {
                    return objs[0] as T;
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// 获取枚举属性
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumSubitems">枚举子项列表</param>
        /// <returns>返回属性列表</returns>
        public static List<T> GetEnumAttributes<T>(params object[] enumSubitems) where T : Attribute, new()
        {
            if (enumSubitems == null || enumSubitems.Length == 0)
            {
                return null;
            }

            List<T> result = new List<T>();

            foreach (var enumSubitem in enumSubitems)
            {
                result.Add(GetEnumAttribute<T>(enumSubitem));
            }

            return result;
        }

        /// <summary>
        /// 获取所有枚举的指定属性列表
        /// </summary>
        /// <typeparam name="T">枚举泛型类型</typeparam>
        /// <typeparam name="U">返回属性泛型类型</typeparam>
        /// <returns>返回属性列表</returns>
        public static List<U> GetEnumAttributes<T, U>() where U : Attribute, new()
        {
            List<U> result = new List<U>();

            Type enumType = typeof(T);
            string[] names = Enum.GetNames(enumType);

            foreach (var name in names)
            {
                result.Add(GetEnumAttribute<U>(Enum.Parse(enumType, name)));
            }

            return result;
        }
    }

    /// <summary>
    /// 扩展属性
    /// </summary>
    public class ExtensionAttribute : Attribute
    {
        private string _description = "";

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public ExtensionAttribute() { }

        public ExtensionAttribute(string description)
        {
            _description = description;
        }
    }
}
