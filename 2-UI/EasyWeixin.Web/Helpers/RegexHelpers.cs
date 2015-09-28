using System;
using System.Text.RegularExpressions;

namespace EasyWeixin.Web.Helpers
{
    public static class RegexHelpers
    {
        /// <summary>
        /// 验证是否数字和字母的组合
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWordAndNum(string value)
        {
            Regex regex = new Regex("[a-zA-Z0-9]?");
            return regex.Match(value).Success;
        }

        public static bool IsEmail(string _value)
        {
            //@"^\w+([-+.]\w+)*@(\w+([-.]\w+)*\.)+([a-zA-Z]+)+_
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }

        /// <summary>
        /// 验证 中国 ID 是否为15位或18位
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool IsValidCNID(string ID)
        {   //验证身份证是否为15位或18位
            Regex regex = new Regex(@"d{18}|d{15}");
            return regex.IsMatch(ID);
        }

        /// <summary>
        /// ID卡
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsIDCard(string _value)
        {
            Regex regex;
            string[] strArray;
            DateTime time;
            if ((_value.Length != 15) && (_value.Length != 0x12))
            {
                return false;
            }
            if (_value.Length == 15)
            {
                regex = new Regex(@"^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})_");
                if (!regex.Match(_value).Success)
                {
                    return false;
                }
                strArray = regex.Split(_value);
                try
                {
                    time = new DateTime(int.Parse("19" + strArray[2]), int.Parse(strArray[3]), int.Parse(strArray[4]));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            regex = new Regex(@"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9Xx])_");
            if (!regex.Match(_value).Success)
            {
                return false;
            }
            strArray = regex.Split(_value);
            try
            {
                time = new DateTime(int.Parse(strArray[2]), int.Parse(strArray[3]), int.Parse(strArray[4]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否 int
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsInt(string _value)
        {
            Regex regex = new Regex(@"^(-){0,1}\d+_");
            if (regex.Match(_value).Success)
            {
                if ((long.Parse(_value) > 0x7fffffffL) || (long.Parse(_value) < -2147483648L))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static bool IsLengthStr(string _value, int _begin, int _end)
        {
            int length = _value.Length;
            if ((length < _begin) && (length > _end))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 只是字符
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool IsOnlyCharacters(string character)
        {
            Regex regex = new Regex(@"^.[A-Za-z]+$");
            return regex.IsMatch(character);
        }

        public static bool IsLetterOrNumber(string _value)
        {
            return QuickValidate("^[a-zA-Z0-9_]*_", _value);
        }

        /// <summary>
        /// 验证中国 手机号
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsCNMobileNum(string _value)
        {
            //13\d{9}_
            Regex regex = new Regex("^1[3|5|6|7|8]\\d{9}$", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }

        /// <summary>
        /// 验证中国 电话
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsCNPhoneNum(string _value)
        {
            //^(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{3,5})?_
            Regex regex = new Regex(@"/^0\d{2,3}-?\d{7,8}$/", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }

        /// <summary>
        /// 验证中国 区号
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidCNZipCode(string zipcode)
        {
            Regex regex = new Regex(@"d{6}");
            return regex.IsMatch(zipcode);
        }

        /// <summary>
        /// 验证美国US电话
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidUSPhone(string phone)
        {
            Regex regex = new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
            return regex.IsMatch(phone);
        }

        /// <summary>
        /// 验证美国zip 区号
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidUSZipCode(string zipcode)
        {
            Regex regex = new Regex(@"\d{5}(-\d{4})?");
            return regex.IsMatch(zipcode);
        }

        /// <summary>
        /// 只是 数字
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsNumber(string _value)
        {
            //^(0|([1-9]+[0-9]*))(.[0-9]+)?_
            return QuickValidate("^.[0-9]*$", _value);
        }

        public static bool IsNumeric(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*_", _value);
        }

        /// <summary>
        /// 验证日期类型为yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsValidDate(string date)
        {   //验证日期类型为yyyy-MM-dd
            Regex regex = new Regex(@"^((((19|20)(([02468][048])|([13579][26]))-02-29))|((20[0-9][0-9])|(19[0-9][0-9]))-((((0[1-9])|(1[0-2]))-((0[1-9])|(1\d)|(2[0-8])))|((((0[13578])|(1[02]))-31)|(((0[1,3-9])|(1[0-2]))-(29|30)))))$");
            return regex.IsMatch(date);
        }

        /// <summary>
        /// 验证日期类型为 yyyy-MM-dd hh:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsValidDateTime(string dateTime)
        {   //验证日期类型为yyyy-MM-dd hh:mm:ss
            Regex regex = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
            return regex.IsMatch(dateTime);
        }

        public static bool IsStringDate(string _value)
        {
            try
            {
                DateTime dTime = DateTime.Parse(_value);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// URL
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsUrl(string _value)
        {
            //(http://)?([\w-]+\.)*[\w-]+(/[\w- ./?%&=]*)?
            Regex regex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }

        public static bool QuickValidate(string _express, string _value)
        {
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }
    }
}