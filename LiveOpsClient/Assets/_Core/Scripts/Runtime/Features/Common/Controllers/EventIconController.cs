using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common.Views;
using App.Runtime.Features.Lobby.Models;
using App.Shared.Mvc;
using App.Shared.Mvc.Services;
using App.Shared.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = App.Shared.Logger.ILogger;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class EventIconController : ControllerBase<EventIconControllerArgs>
    {
        private readonly IControllerService _controllerService;
        private readonly ITimeService _timeService;
        private readonly LiveOpState _state;
        private readonly ILogger _logger;
        private IEventIconView _view;
        private CancellationToken _token;

        public EventIconController(IControllerService controllerService, ITimeService timeService, LiveOpState state, ILogger logger)
        {
            _controllerService = controllerService;
            _timeService = timeService;
            _state = state;
            _logger = logger;
        }

        protected override UniTask OnStart(EventIconControllerArgs args, CancellationToken token)
        {
            try
            {
                _token = token;
                _view = Object.Instantiate(args.IconPrefab, args.IconParent);
                _view.Clicked += OnViewClicked;
                InitializeState();
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Icon flow failed", exception);
            }
            return UniTask.CompletedTask;
        }

        private void InitializeState()
        {
            OnTimeChanged(_timeService.Now);
            if (!_state.IsExpired(_timeService))
                _timeService.TimeChanged += OnTimeChanged;
        }

        private void OnTimeChanged(DateTime now)
        {
            _view.SetTimeLeft(_state.ExpiresIn(_timeService));
            
            if (_state.IsExpired(_timeService))
            {
                _view.Expire();
                _timeService.TimeChanged -= OnTimeChanged;
            }
        }

        private void OnViewClicked()
        {
            Debug.Log("OnViewClicked");
            //_controllerService.StartController<EventPopupController>(_token);
        }

        protected override void OnStop()
        {
            _timeService.TimeChanged -= OnTimeChanged;
            _view.Clicked -= OnViewClicked;
            _view?.Dispose();   
        }
    }
}