using App.Runtime.Features.Common.Views;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.KeyCollectLiveOp.Views
{
    public class KeyCollectLiveOpPopup : EventPopup
    {
        [SerializeField] private TextMeshProUGUI _keysText;
        
        public void SetKeysCollected(int keys)
            => _keysText.text = $"You've collected {keys} keys! Awesome!";
    }
}
