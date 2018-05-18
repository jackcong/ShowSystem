using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Log
{
    public interface ILogger
    {
        // TODO: Add stuff
        void Log(string log, LogEntryType type);
    }
}
