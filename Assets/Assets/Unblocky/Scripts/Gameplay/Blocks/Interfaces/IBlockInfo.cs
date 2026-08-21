using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public interface IBlockInfo
    {
        GameColor Color { get; }
        IReadOnlyList<Vector2Int> OccupiedOffsets { get; }
        GameObject GameObject { get; }
    }

}