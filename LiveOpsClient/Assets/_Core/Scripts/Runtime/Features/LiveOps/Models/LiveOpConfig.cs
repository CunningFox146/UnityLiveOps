using App.Runtime.Features.Common.Views;
using App.Runtime.Utils;
using UnityEngine;

namespace App.Runtime.Features.LiveOps.Models
{
    [CreateAssetMenu(fileName = nameof(LiveOpConfig), menuName = ConfigConstants.LiveOpsPath + nameof(LiveOpConfig))]
    public class LiveOpConfig : ScriptableObject, ILiveOpConfig
    {
        [field: SerializeField] public EventIconView IconPrefab { get; private set; }
        [field: SerializeField] public EventPopup PopupPrefab { get; private set; }
    }
}