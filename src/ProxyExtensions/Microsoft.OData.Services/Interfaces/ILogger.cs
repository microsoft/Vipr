using System;

namespace Microsoft.OData.Services.Interfaces
{
    public interface ILogger
    {
        void Log(String content, LogLevel logLevel);

        void LogFormat(LogLevel logLevel, String format, params object[] args);
    }
}
