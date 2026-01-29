using System;
using System.Threading;
using App.Runtime.Features.LiveOps.Api;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpsCalendarHandler : ILiveOpsCalendarHandler
    {
        private readonly IRepository<LiveOpsCalendar> _repository;
        private readonly ILiveOpsApiService _apiService;
        private readonly ITimeService _timeService;
        private readonly ILogger _logger;

        public LiveOpsCalendar Calendar => _repository.Value;

        public LiveOpsCalendarHandler(
            IRepository<LiveOpsCalendar> repository,
            ILiveOpsApiService apiService,
            ITimeService timeService,
            ILogger logger)
        {
            _repository = repository;
            _apiService = apiService;
            _timeService = timeService;
            _logger = logger;
        }

        public async UniTask LoadFromServer(CancellationToken token)
        {
            try
            {
                await _repository.RestoreFeatureData(token);
                var activeCalendarId = await _apiService.GetCalendarId(token);

                if (IsCalendarUpToDate(activeCalendarId))
                {
                    _logger.Info($"Using cached calendar id: {activeCalendarId}", LoggerTag.LiveOps);
                    return;
                }

                await FetchAndUpdateCalendar(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error("Failed to load LiveOps calendar.", ex, LoggerTag.LiveOps);
            }
        }

        public void SaveCalendar()
            => _repository.Update(Calendar);

        private bool IsCalendarUpToDate(string activeCalendarId)
            => Calendar.Id == activeCalendarId;

        private async UniTask FetchAndUpdateCalendar(CancellationToken token)
        {
            var calendarDto = await _apiService.GetCalendar(token);
            if (calendarDto is not null)
            {
                Calendar.UpdateFromDto(calendarDto, _timeService);
                await _repository.UpdateAsync(Calendar, token);
                _logger.Info("Successfully fetched calendar from server", LoggerTag.LiveOps);
            }
        }
    }
}
