using UnityEngine;

namespace Flavor
{
    public interface IGateInfo
    {
        public Vector3 vPos { get; }
        public GameColor Color { get; }
        public DirectionType Direction { get; }
        public int MaxX { get; }
        public int MaxY { get; }

        public bool IsSatifiedConditions(IBlockInfo blockInfo);
    }
}