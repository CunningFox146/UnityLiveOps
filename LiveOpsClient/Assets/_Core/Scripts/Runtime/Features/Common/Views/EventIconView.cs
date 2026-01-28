using System;
using App.Shared.Utils;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.Common.Views
{
    public abstract class EventIconView : MonoBehaviour, IEventIconView
    {
        public event Action Clicked;
        [SerializeField] private TextMeshProUGUI _timer;

        public virtual void SetTimeLeft(TimeSpan timeLeft)
        {
            _timer.text = timeLeft.ToString(@"hh\:mm\:ss");
        }

        public abstract void Expire();
        
        public virtual void OnClick()
            => Clicked?.Invoke();

        public void Dispose()
            => this.DestroyBehaviourObject();
    }
}