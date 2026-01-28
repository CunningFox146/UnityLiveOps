using System;
using System.Threading;
using App.Runtime.Features.Lobby.Models;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Runtime.Features.Common.Views
{
    public abstract class EventPopup : MonoBehaviour, IEventPopup
    {
        public event Action CtaClicked;
        
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _ctaText;
        [SerializeField] protected Button _ctaButton;
        
        public void SetTitle(string title)
            => _title.text = title;
        
        public void SetCtaText(string ctaText)
            => _ctaText.text = ctaText;
        
        public void OnCtaClicked()
            => CtaClicked?.Invoke();

        public UniTask WaitForCtaClick(CancellationToken token)
            => _ctaButton.OnClickAsync(token);

        public void Dispose()
            => this.DestroyBehaviourObject();
    }
}