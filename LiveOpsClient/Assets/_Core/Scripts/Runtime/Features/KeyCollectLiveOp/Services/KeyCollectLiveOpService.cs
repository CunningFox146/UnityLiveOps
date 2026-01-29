using System;
using System.Threading;
using App.Runtime.Features.KeyCollectLiveOp.Model;
using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Calendar;
using App.Runtime.Gameplay.Models;
using App.Runtime.Gameplay.Services;
using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.KeyCollectLiveOp.Services
{
    public class KeyCollectLiveOpService : IKeyCollectLiveOpService
    {
        private readonly IRepository<KeyCollectLiveOpData> _repository;
        private readonly LiveOpState _state;
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly ILiveOpsCalendarHandler _calendarHandler;
        private readonly IGameplayHandler _gameplayHandler;
        private readonly ILogger _logger;
        public int KeysCollected => Data.KeysCollected;
        private KeyCollectLiveOpData Data => _repository.Value;

        public KeyCollectLiveOpService(IRepository<KeyCollectLiveOpData> repository, LiveOpState state,
            ITimeService timeService, IFeatureService featureService, ILiveOpsCalendarHandler calendarHandler,
            IGameplayHandler gameplayHandler, ILogger logger)
        {
            _repository = repository;
            _state = state;
            _timeService = timeService;
            _featureService = featureService;
            _calendarHandler = calendarHandler;
            _gameplayHandler = gameplayHandler;
            _logger = logger;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            _gameplayHandler.GameplayExit += OnGameplayExit;
            try
            {
                await _repository.RestoreFeatureData(token);
                
                // Clear old data if it belongs to a previous event occurrence
                if (Data.EventStartTime != _state.StartTime)
                {
                    _repository.Clear();
                    Data.EventStartTime = _state.StartTime;
                    _repository.Update(Data);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to initialize KeyCollect LiveOp Service", exception, LoggerTag.LiveOps);
            }
        }

        private void OnGameplayExit(GameplaySession session)
        {
            Data.KeysCollected += session.KeysCollected;
            _repository.Update(Data);
        }

        public void TryUnloadFeatureIfExpired()
        {
            if (!_state.IsExpired(_timeService))
                return;
            
            _featureService.StopFeature(_state.Type);
            _calendarHandler.RemoveSeenEvent(_state);
            _repository.Clear();
        }
    }
}
