using System;
using System.Diagnostics;
using Cysharp.Text;
using UnityEngine;

namespace App.Shared.Logger
{
    public class UnityLogger : ILogger
    {
        private const string ColorDebug = "#888888";
        private const string ColorInfo = "#AAAAAA";
        private const string ColorWarn = "#FFD700";
        private const string ColorError = "#FF6B6B";
        private const string ColorTag = "#4FC3F7";

        [HideInCallstack]
        public void Debug(string message, LoggerTag tag = LoggerTag.Generic)
            => InternalDebug(message, tag);

        [HideInCallstack]
        public void Info(string message, LoggerTag tag = LoggerTag.Generic)
            => UnityEngine.Debug.Log(FormatMessage(tag, message, ColorInfo));

        [HideInCallstack]
        public void Warn(string message, Exception exception = null, LoggerTag tag = LoggerTag.Generic)
        {
            var msg = exception is not null 
                ? ZString.Concat(message, "\n", FormatException(exception)) 
                : message;
            UnityEngine.Debug.LogWarning(FormatMessage(tag, msg, ColorWarn));
        }

        [HideInCallstack]
        public void Error(string message, Exception exception = null, LoggerTag tag = LoggerTag.Generic)
        {
            var msg = exception is not null 
                ? ZString.Concat(message, "\n", FormatException(exception)) 
                : message;
            UnityEngine.Debug.LogError(FormatMessage(tag, msg, ColorError));
        }

        [HideInCallstack]
        public void LogUniTask(Exception exception)
        {
            if (exception is not OperationCanceledException)
            {
                Error(exception.Message, exception);
            }
        }
        
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEBUG")]
        [HideInCallstack]
        private static void InternalDebug(string message, LoggerTag tag)
            => UnityEngine.Debug.Log(FormatMessage(tag, message, ColorDebug));

        private static string FormatMessage(LoggerTag tag, string message, string color)
        {
            return ZString.Concat(
                "<color=", ColorTag, ">[", tag, "]</color> <color=", color, ">", message, "</color>"
            );
        }

        private static string FormatException(Exception exception)
        {
            if (exception is null)
                return string.Empty;

            using var sb = ZString.CreateStringBuilder();
            
            sb.Append("<b>");
            sb.Append(exception.GetType().Name);
            sb.Append("</b>: ");
            sb.Append(exception.Message);

            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                sb.Append('\n');
                sb.Append(exception.StackTrace);
            }

            if (exception.InnerException != null)
            {
                sb.Append("\n<b>Inner:</b> ");
                sb.Append(exception.InnerException.GetType().Name);
                sb.Append(": ");
                sb.Append(exception.InnerException.Message);
            }

            return sb.ToString();
        }
    }
}
