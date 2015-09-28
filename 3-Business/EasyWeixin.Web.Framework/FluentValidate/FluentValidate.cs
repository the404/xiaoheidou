using System.Collections.Generic;
using System.Web.Mvc;

namespace EasyWeixin.Web.Framework.FluentValidate
{
    public static class FluentValidate
    {
        public static void AddModelFluentErrors(this ModelStateDictionary modelState, IList<FluentValidation.Results.ValidationFailure> validationResults, string defaultErrorKey = null)
        {
            if (validationResults == null) return;

            foreach (var validationResult in validationResults)
            {
                string key = validationResult.PropertyName ?? defaultErrorKey ?? string.Empty;
                modelState.AddModelError(key, validationResult.ErrorMessage);
            }
        }
    }
}