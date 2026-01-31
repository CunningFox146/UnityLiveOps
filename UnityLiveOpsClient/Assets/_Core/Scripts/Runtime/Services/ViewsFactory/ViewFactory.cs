using App.Runtime.Services.Cameras;
using App.Runtime.Services.ViewStack;
using UnityEngine;

namespace App.Runtime.Services.ViewsFactory
{
    public class ViewFactory : IViewFactory
    {
        private readonly ICameraProvider _cameraProvider;
        private readonly IViewStack _viewStack;

        public ViewFactory(ICameraProvider cameraProvider, IViewStack viewStack)
        {
            _cameraProvider = cameraProvider;
            _viewStack = viewStack;
        }
        
        public TView CreateView<TView>(TView prefab)
            where TView : Object
        {
            var view = Object.Instantiate(prefab);
            
            if (view is ICameraAware initializable)
                initializable.SetCamera(_cameraProvider);

            if (view is ICloseableView closeableView)
                RegisterViewInStack(closeableView);

            return view;
        }

        private void RegisterViewInStack(ICloseableView view)
        {
            _viewStack.Push(view);
            view.ViewClosed += OnViewClosed;
            
            if (view is ISortableView sortableView)
                sortableView.SetSortingOrder(_viewStack.ViewsCount);

            void OnViewClosed()
            {
                view.ViewClosed -= OnViewClosed;
                if (_viewStack.TopView == view)
                    _viewStack.Pop(false);
            }
        }
    }
}