using System.Threading;
using System.Threading.Tasks;
using App.Runtime.Features.Common.Views;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.KeyCollectLiveOp.Views
{
    public class KeyCollectLiveOpPopup : EventPopup
    {
        [SerializeField] private TextMeshProUGUI _keysText;
        [SerializeField] private Canvas _canvas;
        
        public void SetKeysCollected(int keys)
            => _keysText.text = $"You've collected {keys} keys! Awesome!";

        public void SetCamera(Camera canvasCamera)
            => _canvas.worldCamera = canvasCamera;
    }
}
