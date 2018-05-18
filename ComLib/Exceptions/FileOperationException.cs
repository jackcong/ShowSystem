using ComLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Exceptions
{
    [ExceptionAttribute("SendMessageBackAndLogInSystem")]
    public class FileOperationException: Exception
    {
        public string _message = "The file {0} is not exist in system, please contact administrator to get help.";
        public FileOperationException(string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
                string.Format("{0}", _message);
        }
        public FileOperationException()
        {
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
