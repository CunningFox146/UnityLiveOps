using System;
using Cysharp.Text;

namespace App.Shared.Utils
{
    public static class ExceptionUtils
    {
        public static string AggregateInnerExceptions(this Exception exception, bool messageOnly)
        {
            using var message = ZString.CreateStringBuilder();
            var current = exception;

            while (current is not null)
            {
                var value = ToString(current);
                message.Append(value);
                current = current.InnerException;
            }

            var result = message.ToString();
            return result;

            string ToString(Exception e)
            {
                if (!messageOnly)
                {
                    return e.ToString();
                }

                return !string.IsNullOrEmpty(e.Message) ? e.Message : string.Empty;
            }
        }
    }
}