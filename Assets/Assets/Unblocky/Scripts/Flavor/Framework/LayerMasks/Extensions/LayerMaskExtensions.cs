using UnityEngine;

namespace Flavor
{
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Ki?m tra xem LayerMask có ch?a n?m trong Layer hay không.
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) != 0; 
        }

        /// <summary>
        /// Ki?m tra xem GameObject có n?m trong LayerMask hay không.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static bool IsInLayerMask(this GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }
    }
}