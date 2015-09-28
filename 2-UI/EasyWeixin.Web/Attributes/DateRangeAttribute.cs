using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyWeixin.Web.Attributes
{
    public class DateRangeAttribute : RangeAttribute, IClientValidatable
    {
        public DateRangeAttribute(string minimumValue)
            : base(typeof(DateTime), minimumValue, DateTime.Now.ToShortDateString())
        {

        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "range"
            };

            rule.ValidationParameters.Add("min", Minimum);
            rule.ValidationParameters.Add("max", Maximum);

            yield return rule;
        }
    }
}