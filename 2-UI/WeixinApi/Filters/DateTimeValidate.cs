using System;
using System.ComponentModel.DataAnnotations;

namespace WeixinApi.Filters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateTimeValidateAttribute : ValidationAttribute
    {
        public DateTimeValidateAttribute()
        {
            //set default max and min time according to sqlserver datetime type
            MinDate = new DateTime(1753, 1, 1).ToString();
            MaxDate = new DateTime(9999, 12, 31).ToString();
        }

        public string MinDate
        {
            get;
            set;
        }

        public string MaxDate
        {
            get;
            set;
        }

        private DateTime minDate, maxDate;

        //indicate if the format of the input is really a datetime
        private bool isFormatError = true;

        public override bool IsValid(object value)
        {
            //ignore it if no value
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;
            //begin check
            string s = value.ToString();
            minDate = DateTime.Parse(MinDate);
            maxDate = DateTime.Parse(MaxDate);
            bool result = true;
            try
            {
                DateTime date = DateTime.Parse(s);
                isFormatError = false;
                if (date > maxDate || date < minDate)
                    result = false;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            if (isFormatError)
                return "请输入合法的日期";
            return base.FormatErrorMessage(name);
        }
    }
}