using System;
using System.Collections.Generic;
using App.Runtime.Services.Input;
using VContainer.Unity;

namespace App.Runtime.Services.ViewStack
{
    public class ViewStack : IViewStack, IInitializable, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly Stack<ICloseableView> _views = new();

        public ViewStack(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Initialize()
        {
            _inputService.BackPressed += OnBackPressed;
        }

        public void Dispose()
        {
            _inputService.BackPressed -= OnBackPressed;
        }

        public void Push(ICloseableView view)
        {
            _views.Push(view);
        }

        public void Pop()
        {
            if (_views.TryPop(out var view))
                view.RequestClose();
        }

        private void OnBackPressed()
            => Pop();
    }
}