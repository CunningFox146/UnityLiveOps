using System;
using App.Runtime.Input;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace App.Runtime.Services.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        public event Action BackPressed;
        
        private readonly InputSystemActions _actions = new();

        public void Initialize()
        {
            _actions.Enable();
            _actions.UI.Back.performed += OnBackPerformed;
        }

        public void Dispose()
        {
            _actions.Disable();
            _actions.Dispose();
        }
        
        private void OnBackPerformed(InputAction.CallbackContext ctx)
            => BackPressed?.Invoke();
    }
}