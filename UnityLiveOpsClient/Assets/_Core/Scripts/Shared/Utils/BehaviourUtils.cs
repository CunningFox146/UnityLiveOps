using UnityEngine;

namespace App.Shared.Utils
{
    public static class BehaviourUtils
    {
        public static void DestroyBehaviourObject(this Behaviour behaviour)
        {
            if (behaviour != null)
                Object.Destroy(behaviour.gameObject);
        }
    }
}