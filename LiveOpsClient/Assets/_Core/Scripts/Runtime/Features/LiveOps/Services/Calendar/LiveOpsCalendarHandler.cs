using System;
using System.Threading;
using App.Runtime.Features.LiveOps.Api;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services.Calendar
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

        public async UniTask LoadCalendar(CancellationToken token)
        {
            try
            {
                await _repository.RestoreFeatureData(token);
                await UpdateCalendar(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error("Failed to load LiveOps calendar", ex, LoggerTag.LiveOps);
            }
        }
        
        public void SaveCalendar()
            => _repository.Update(Calendar);

        public void RemoveSeenEvent(LiveOpState state)
        {
            Calendar.SeenEvents.Remove(state.Type);
            SaveCalendar();
        }

        private async UniTask UpdateCalendar(CancellationToken token)
        {
            try
            {
                var activeCalendarId = await _apiService.GetCalendarId(token);

                if (IsCalendarUpToDate(activeCalendarId))
                {
                    _logger.Info($"Using cached calendar {activeCalendarId}", LoggerTag.LiveOps);
                    return;
                }

                await FetchAndUpdateCalendar(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error("Failed to fetch calendar, using cached one", ex, LoggerTag.LiveOps);
            }
        }
        
        private async UniTask FetchAndUpdateCalendar(CancellationToken token)
        {
            try
            {
                var calendarDto = await _apiService.GetCalendar(token);
                Calendar.UpdateFromDto(calendarDto, _timeService);
                SaveCalendar();
                _logger.Info("Successfully updated calendar from server", LoggerTag.LiveOps);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error("Failed to update calendar", ex, LoggerTag.LiveOps);
            }
        }
        
        private bool IsCalendarUpToDate(string activeCalendarId)
            => Calendar.Id == activeCalendarId;
    }
}
