using System;

namespace Core.Infrastructure.Logger
{
    public interface ILogger
    {
        void Debug(LoggerTag tag, string message);
        void Info(LoggerTag tag, string message);
        void Warn(LoggerTag tag, string message, Exception exception = null);
        void Error(LoggerTag tag, string message, Exception exception = null);
        void LogUniTask(Exception exception);
    }
}
