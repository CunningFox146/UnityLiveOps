using System.Threading;
using System.Threading.Tasks;
using App.Runtime.Features.Common.Views;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.ClickerLiveOp.Views
{
    public class ClickerLiveOpPopup : EventPopup
    {
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Canvas _canvas;
        
        public void SetProgress(int progress)
            => _progressText.text = $"You've clicked {progress} times! Crazy!";

        public void SetCamera(Camera canvasCamera)
            => _canvas.worldCamera = canvasCamera;
    }
}