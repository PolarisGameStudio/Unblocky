using UnityEngine;
using System.Linq;
using System.Collections.Generic;
namespace Flavor
{
    public class VectorUtils
    {
        public static (int minX, int maxX, int minY, int maxY) GetMinMax(List<Vector2Int> list)
        {
            if(list == null || list.Count == 0)
            {
                // Trả về 0 nếu list rỗng
                return (0, 0, 0, 0);
            }

            int minX = list.Min(v => v.x);
            int maxX = list.Max(v => v.x);
            int minY = list.Min(v => v.y);
            int maxY = list.Max(v => v.y);

            return (minX, maxX, minY, maxY);
        }
    }
}