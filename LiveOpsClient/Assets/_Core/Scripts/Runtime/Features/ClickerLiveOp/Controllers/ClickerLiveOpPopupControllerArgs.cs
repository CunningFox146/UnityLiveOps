using System;
using App.Runtime.Features.ClickerLiveOp.Views;

namespace App.Runtime.Features.ClickerLiveOp.Controllers
{
    public readonly struct ClickerLiveOpPopupControllerArgs
    {
        public ClickerLiveOpPopup PopupPrefab { get; }
        
        public ClickerLiveOpPopupControllerArgs(ClickerLiveOpPopup popupPrefab, Action ctaClicked)
        {
            PopupPrefab = popupPrefab;
        }
    }
}