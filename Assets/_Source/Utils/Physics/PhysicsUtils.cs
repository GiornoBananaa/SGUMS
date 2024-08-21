using UnityEngine;

namespace Utils
{
    public static class PhysicsUtils
    {
        public static bool ContainsLayer(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}
