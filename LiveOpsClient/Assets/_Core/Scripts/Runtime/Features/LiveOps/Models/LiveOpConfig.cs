using App.Runtime.Utils;
using UnityEngine;

namespace App.Runtime.Features.LiveOps.Models
{
    public interface ILiveOpConfig
    {
        GameObject IconPrefab { get; set; }
    }

    [CreateAssetMenu(fileName = nameof(LiveOpConfig), menuName = ConfigConstants.LiveOpsPath + nameof(LiveOpConfig))]
    public class LiveOpConfig : ScriptableObject, ILiveOpConfig
    {
        [field: SerializeField] public GameObject IconPrefab { get; set; }
    }
}