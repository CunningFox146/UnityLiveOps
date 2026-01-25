using System;

namespace App.Shared.Logger
{
    public interface ILogger
    {
        void Debug(string message, LoggerTag tag = LoggerTag.Generic);
        void Info(string message, LoggerTag tag = LoggerTag.Generic);
        void Warn(string message, Exception exception = null, LoggerTag tag = LoggerTag.Generic);
        void Error(string message, Exception exception = null, LoggerTag tag = LoggerTag.Generic);
        void LogUniTask(Exception exception);
    }
}
