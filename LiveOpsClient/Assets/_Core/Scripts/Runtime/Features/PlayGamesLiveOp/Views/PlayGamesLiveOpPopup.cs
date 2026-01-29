using System.Threading;
using System.Threading.Tasks;
using App.Runtime.Features.Common.Views;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.PlayGamesLiveOp.Views
{
    public class PlayGamesLiveOpPopup : EventPopup
    {
        [SerializeField] private TextMeshProUGUI _gamesText;
        [SerializeField] private Canvas _canvas;
        
        public void SetGamesPlayed(int games)
            => _gamesText.text = $"You've played {games} games! Amazing!";

        public void SetCamera(Camera canvasCamera)
            => _canvas.worldCamera = canvasCamera;
    }
}
