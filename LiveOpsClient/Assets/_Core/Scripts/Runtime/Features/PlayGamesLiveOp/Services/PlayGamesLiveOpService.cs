using System;
using System.Threading;
using App.Runtime.Features.PlayGamesLiveOp.Model;
using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Calendar;
using App.Runtime.Gameplay.Models;
using App.Runtime.Gameplay.Services;
using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.PlayGamesLiveOp.Services
{
    public class PlayGamesLiveOpService : IPlayGamesLiveOpService
    {
        private readonly IRepository<PlayGamesLiveOpData> _repository;
        private readonly LiveOpState _state;
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly ILiveOpsCalendarHandler _calendarHandler;
        private readonly IGameplayHandler _gameplayHandler;
        private readonly ILogger _logger;
        public int GamesPlayed => Data.GamesPlayed;
        private PlayGamesLiveOpData Data => _repository.Value;

        public PlayGamesLiveOpService(IRepository<PlayGamesLiveOpData> repository, LiveOpState state,
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
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to initialize PlayGames LiveOp Service", exception, LoggerTag.LiveOps);
            }
        }

        private void OnGameplayExit(GameplaySession _)
        {
            Data.GamesPlayed++;
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
