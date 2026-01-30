using App.Runtime.Features.Common.Views;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.PlayGamesLiveOp.Views
{
    public class PlayGamesLiveOpPopup : EventPopup
    {
        [SerializeField] private TextMeshProUGUI _gamesText;
        
        public void SetGamesPlayed(int games)
            => _gamesText.text = $"You've played {games} games! Mindblowing!";
    }
}
