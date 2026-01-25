using System;
using System.Threading.Tasks;
using Common.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using ILogger = Common.Logger.ILogger;

namespace Common.Monitoring
{
    public class UnhandledExceptionMonitoringService : IInitializable, IDisposable
    {
        private readonly ILogger _logger;

        public UnhandledExceptionMonitoringService(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            TaskScheduler.UnobservedTaskException += ReportUnobservedTaskException;
            UniTaskScheduler.UnobservedTaskException += ReportUnobservedUniTaskException;
            AppDomain.CurrentDomain.UnhandledException += ReportUnhandledException;
        }

        public void Dispose()
        {
            TaskScheduler.UnobservedTaskException -= ReportUnobservedTaskException;
            UniTaskScheduler.UnobservedTaskException -= ReportUnobservedUniTaskException;
            AppDomain.CurrentDomain.UnhandledException -= ReportUnhandledException;
        }

        [HideInCallstack]
        private void ReportUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            args.SetObserved();
            foreach (var innerException in args.Exception.InnerExceptions)
                if (innerException is not OperationCanceledException)
                    _logger.Error(
                        $"Unobserved exception in {sender.GetType()}, {innerException.AggregateInnerExceptions(false)}");
        }

        [HideInCallstack]
        private void ReportUnobservedUniTaskException(Exception innerException)
        {
            if (innerException is not OperationCanceledException)
                _logger.Error("Unobserved exception", innerException);
        }

        [HideInCallstack]
        private void ReportUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject.ToString());
            _logger.Error(
                $"Unobserved exception in {sender.GetType()}, {ex.AggregateInnerExceptions(false)}");
        }
    }
}