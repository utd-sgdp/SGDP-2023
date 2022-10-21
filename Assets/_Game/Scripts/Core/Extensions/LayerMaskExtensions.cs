using UnityEngine;

namespace Game
{
    public static class LayerMaskExtensions
    {
        public static bool Includes(this LayerMask mask, int layer)
        {
            return ((mask >> layer) & 1) == 1;
        }
    }
}
