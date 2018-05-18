using ComLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Exceptions
{
    [ExceptionAttribute("SendEmailAndLogInSystem")]
    public class CustomeOutRangeException: Exception
    {
        public string _message = "Index out of range exception happen.";
        public CustomeOutRangeException(string message)
        {
            _message = message;
        }
        public override string Message
        {
            get
            {
                return this._message;
            }
        }
        public string OperationType { get; set; }
    }
}
