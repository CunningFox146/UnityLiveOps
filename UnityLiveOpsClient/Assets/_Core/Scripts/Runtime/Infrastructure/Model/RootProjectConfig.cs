using App.Runtime.Utils;
using UnityEngine;

namespace App.Runtime.Infrastructure.Model
{
    [CreateAssetMenu(fileName = nameof(RootProjectConfig), menuName = ConfigConstants.ConfigPath + nameof(RootProjectConfig), order = -999)]
    public class RootProjectConfig : ScriptableObject
    {
        [field: SerializeField] public string EnvironmentUrl { get; private set; }
    }
}