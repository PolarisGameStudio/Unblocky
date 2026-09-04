using UnityEngine;

namespace Flavor
{
    public static class DirectionExtensions
    {
        public static Vector3 ToWorldVector(this DirectionType direction)
        {
            return direction switch
            {
                DirectionType.Left => Vector3.left,
                DirectionType.Right => Vector3.right,
                DirectionType.Top => Vector3.forward,
                DirectionType.Bot => Vector3.back,
                _ => Vector3.zero
            };
        }

        public static Vector2Int ToGridVector(this DirectionType direction)
        {
            return direction switch
            {
                DirectionType.Left => new Vector2Int(-1, 0),
                DirectionType.Right => new Vector2Int(1, 0),
                DirectionType.Top => new Vector2Int(0, 1),
                DirectionType.Bot => new Vector2Int(0, -1),
                _ => Vector2Int.zero
            };
        }
        public static int GetExitDistance(this DirectionType direction, Vector2Int blockSize)
        {
            // Nếu cổng nằm ngang (Trái, Phải), quãng đường đi bằng chiều RỘNG (X) của cục gạch
            if (DirectionType.Horizontal.HasFlag(direction)) return blockSize.x;

            // Nếu cổng nằm dọc (Trên, Dưới), quãng đường đi bằng chiều DÀI (Y/Z) của cục gạch
            if (DirectionType.Vertical.HasFlag(direction)) return blockSize.y;

            return 1; // Mặc định là 1 ô
        }
    }
}