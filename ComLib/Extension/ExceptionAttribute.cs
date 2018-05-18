using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Extension
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExceptionAttribute: Attribute
    {
        public string _operationType;
        public ExceptionAttribute(string operationtype)
        {
            _operationType = operationtype;
        }
    }
}
