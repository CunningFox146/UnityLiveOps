using System;
using App.Shared.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Runtime.Features.Common.Views
{
    public abstract class EventIconView : MonoBehaviour, IEventIconView
    {
        public event Action Clicked;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private Image[] _imagesToGrayscale;
        [SerializeField] private Material _grayscaleMaterial;

        public virtual void SetTimeLeft(TimeSpan timeLeft)
        {
            _timer.text = timeLeft.ToString(@"hh\:mm\:ss");
        }

        public virtual void Expire()
        {
            foreach (var image in _imagesToGrayscale)
            {
                image.material = _grayscaleMaterial;
            }

            _timer.text = "Finished";
        }
        
        public virtual void OnClick()
            => Clicked?.Invoke();

        public void Dispose()
            => this.DestroyBehaviourObject();
    }
}