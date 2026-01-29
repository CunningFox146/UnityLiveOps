using System;
using System.Threading;
using App.Runtime.Features.UserState.Services;
using App.Runtime.Gameplay.Models;
using App.Runtime.Services.SceneLoader;
using App.Shared.Logger;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Gameplay.Services
{
    public class GameplayService : IGameplayService, IInitializable, IDisposable
    {
        private readonly IGameplayHandler _gameplayHandler;
        private readonly ISceneLoaderService _sceneLoader;
        private readonly IUserStateService _userState;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private CancellationToken Token => _cancellationTokenSource.Token;

        public GameplayService(IGameplayHandler gameplayHandler, ISceneLoaderService sceneLoader, IUserStateService userState, ILogger logger)
        {
            _gameplayHandler = gameplayHandler;
            _sceneLoader = sceneLoader;
            _userState = userState;
            _logger = logger;
        }

        public void Initialize()
        {
            _gameplayHandler.GameplayEnter += OnGameplayEnter;
            _gameplayHandler.GameplayExit += OnGameplayExit;
        }

        public void Dispose()
        {
            _gameplayHandler.GameplayEnter -= OnGameplayEnter;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
        
        private void OnGameplayEnter()
        {
            _logger.Info("Entering gameplay");
            _gameplayHandler.ClearSession();
            LoadGameplaySceneAsync(Token).Forget(_logger.LogUniTask);
        }
        
        private void OnGameplayExit(GameplaySession session)
        {
            _userState.SetCurrentLevel(_userState.CurrentLevel + 1);
            LoadLobbySceneAsync(Token).Forget(_logger.LogUniTask);
        }
        
        private async UniTask LoadGameplaySceneAsync(CancellationToken token)
        {
            try
            {
                await _sceneLoader.LoadSceneAsync("Gameplay", cancellationToken: token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to load gameplay scene", exception, LoggerTag.Gameplay);
            }
        }
        
        private async UniTask LoadLobbySceneAsync(CancellationToken token)
        {
            try
            {
                await _sceneLoader.LoadSceneAsync("Lobby", cancellationToken: token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to load lobby scene", exception, LoggerTag.Gameplay);
            }
        }
        
    }
}