using App.Runtime.Features.Common.Views;
using App.Runtime.Utils;
using UnityEngine;

namespace App.Runtime.Features.LiveOps.Models
{
    public interface ILiveOpConfig
    {
        EventIconView IconPrefab { get; set; }
    }

    [CreateAssetMenu(fileName = nameof(LiveOpConfig), menuName = ConfigConstants.LiveOpsPath + nameof(LiveOpConfig))]
    public class LiveOpConfig : ScriptableObject, ILiveOpConfig
    {
        [field: SerializeField] public EventIconView IconPrefab { get; set; }
    }
}