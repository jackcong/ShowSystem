using ComLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Exceptions
{
    [ExceptionAttribute("RollBackAndSendEmail")]
    public class DBException: Exception
    {
        public string _message = "DB exception happen.";
        public DBException(string message)
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
