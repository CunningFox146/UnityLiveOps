using App.Runtime.Features.Common.Views;

namespace App.Runtime.Features.LiveOps.Models
{
    public interface ILiveOpConfig
    {
        EventIconView IconPrefab { get; }
        EventPopup PopupPrefab { get; }
    }
}