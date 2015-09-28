using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.ExceptionOperation
{
    public class GException : Exception
    {
        private string _code;
        private string _message;
        /// <summary>
        /// 异常消息
        /// </summary>
        public new string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string ResponseCode
        {
            set { _code = value; }
            get { return _code; }
        }

        private GException()
            : base()
        { }

        public GException(string message)
            : base(message)
        {
            _message = message;
        }

        public GException(string message, Exception innerException)
            : base(message, innerException)
        {
            _message = message;
        }
    }
}
