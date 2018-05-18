using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ComLib.Log
{
    public class WinEventLogger : ILogger
    {
        private readonly string _eventSource;
        public WinEventLogger(string eventSource)
        {
            _eventSource = eventSource;
        }
        protected EventLogEntryType GetEventLogEntryType(LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.Error:
                    return EventLogEntryType.Error;
                case LogEntryType.Warning:
                    return EventLogEntryType.Warning;
                case LogEntryType.Info:
                case LogEntryType.Debug:
                    return EventLogEntryType.Information;
                default:
                    return EventLogEntryType.Information;
            }
        }

        public void Log(string log, LogEntryType type)
        {
            if (!EventLog.SourceExists(_eventSource))
                EventLog.CreateEventSource(_eventSource, "Application");
            EventLog.WriteEntry(_eventSource, log, GetEventLogEntryType(type));
        }
    }
}
