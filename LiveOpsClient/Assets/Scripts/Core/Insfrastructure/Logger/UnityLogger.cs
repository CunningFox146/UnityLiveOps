using System;
using UnityEngine;

namespace Core.Infrastructure.Logger
{
    public class UnityLogger : ILogger
    {
        private const string ColorDebug = "#888888";
        private const string ColorInfo = "#AAAAAA";
        private const string ColorWarn = "#FFD700";
        private const string ColorError = "#FF6B6B";
        private const string ColorTag = "#4FC3F7";

        [HideInCallstack]
        public void Debug(LoggerTag tag, string message)
        {
            UnityEngine.Debug.Log(FormatMessage(tag, message, ColorDebug));
        }

        [HideInCallstack]
        public void Info(LoggerTag tag, string message)
        {
            UnityEngine.Debug.Log(FormatMessage(tag, message, ColorInfo));
        }

        [HideInCallstack]
        public void Warn(LoggerTag tag, string message, Exception exception = null)
        {
            var msg = exception != null ? $"{message}\n{FormatException(exception)}" : message;
            UnityEngine.Debug.LogWarning(FormatMessage(tag, msg, ColorWarn));
        }

        [HideInCallstack]
        public void Error(LoggerTag tag, string message, Exception exception = null)
        {
            var msg = exception != null ? $"{message}\n{FormatException(exception)}" : message;
            UnityEngine.Debug.LogError(FormatMessage(tag, msg, ColorError));
        }

        [HideInCallstack]
        public void LogUniTask(Exception exception)
        {
            if (exception is not OperationCanceledException)
            {
                Error(LoggerTag.Generic, exception.Message, exception);
            }
        }

        private static string FormatMessage(LoggerTag tag, string message, string color)
        {
            return $"<color={ColorTag}>[{tag}]</color> <color={color}>{message}</color>";
        }

        private static string FormatException(Exception exception)
        {
            if (exception == null) return string.Empty;

            var message = $"<b>{exception.GetType().Name}</b>: {exception.Message}";

            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                message += $"\n{exception.StackTrace}";
            }

            if (exception.InnerException != null)
            {
                message += $"\n<b>Inner:</b> {exception.InnerException.GetType().Name}: {exception.InnerException.Message}";
            }

            return message;
        }
    }
}
