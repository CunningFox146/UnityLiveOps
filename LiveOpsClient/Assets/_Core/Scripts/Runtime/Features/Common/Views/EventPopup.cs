using System;
using System.Threading;
using App.Runtime.Services.Cameras;
using App.Runtime.Services.ViewsFactory;
using App.Runtime.Services.ViewStack;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Runtime.Features.Common.Views
{
    public abstract class EventPopup : MonoBehaviour, IEventPopup, ICloseableView, ISortableView, ICameraAware
    {
        public event Action ViewClosed;
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _ctaText;
        [SerializeField] protected Button _ctaButton;
        
        public UniTask WaitForCtaClick(CancellationToken token) =>
            _ctaButton.OnClickAsync(token);

        public void SetTitle(string title)
            => _title.text = title;
        
        public void SetCtaText(string ctaText)
            => _ctaText.text = ctaText;

        public void RequestClose()
            => _ctaButton.onClick.Invoke();
        
        public void SetCamera(ICameraProvider provider)
            => _canvas.worldCamera = provider.UICamera;

        public void SetSortingOrder(int order)
            => _canvas.sortingOrder = order;
        
        public void Dispose()
        {
            this.DestroyBehaviourObject();
            ViewClosed?.Invoke();
        }
    }
}