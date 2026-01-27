using System;
using System.Threading;
using App.Shared.Api;
using App.Shared.Logger;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpsApiService : ApiServiceBase, ILiveOpsApiService
    {
        private readonly ILogger _logger;

        public LiveOpsApiService(IHttpClient httpClient, ILogger logger) : base(httpClient)
        {
            _logger = logger;
        }

        public async UniTask<LiveOpsCalendarDto> GetCalendar(CancellationToken token = default)
        {
            try
            {
                return await GetAsync<LiveOpsCalendarDto>("LiveOps/Active", token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to get active calendar", exception, LoggerTag.LiveOps);
            }

            return null;
        }

        public async UniTask<Guid> GetCalendarId(CancellationToken token = default)
        {
            try
            {
                return await GetAsync<Guid>("LiveOps/ActiveId", token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to get active calendar id", exception, LoggerTag.LiveOps);
            }

            return Guid.Empty;
        }
    }
}