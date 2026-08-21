using UnityEngine;
using System.Linq;
using System.Collections.Generic;
namespace Flavor
{
    public class VectorUtils
    {
        public static (int maxX, int maxY) GetMax(IEnumerable<Vector2Int> list)
        {
            if (list == null || !list.Any())
            {
                return (0, 0);
            }
            int maxX = list.Max(v => v.x);
            int maxY = list.Max(v => v.y);
            return (maxX, maxY);
        }
    }
}