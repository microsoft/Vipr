using Microsoft.OData.Services.Interfaces;
using System;

namespace Microsoft.OData.Services.Utility
{
    public abstract class LoggerBase : ILogger
    {
        private LogLevel LogLevel { get; set; }

        private bool Enabled { get; set; }

        protected LoggerBase()
        {
            LogLevel = LogLevel.Error;
            Enabled = true;
        }

        public void Log(string content, LogLevel logLevel)
        {
            if (!Enabled)
            {
                return;
            }

            if ((LogLevel & logLevel) != logLevel)
            {
                return;
            }

            if (!string.IsNullOrEmpty(content))
            {
                Print(content, logLevel);
            }
        }

        public void LogFormat(LogLevel logLevel, string format, params object[] args)
        {
            Print(string.Format(format, args), logLevel);
        }

        public abstract void Print(String content, LogLevel logLevel);
    }
}
