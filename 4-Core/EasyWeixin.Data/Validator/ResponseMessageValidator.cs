using EasyWeixin.Model;
using FluentValidation;

namespace EasyWeixin.Data.Validator
{
    public class ResponseMessageValidator : AbstractValidator<ResponseMessage>
    {
        public ResponseMessageValidator()
        {
        }
    }
}