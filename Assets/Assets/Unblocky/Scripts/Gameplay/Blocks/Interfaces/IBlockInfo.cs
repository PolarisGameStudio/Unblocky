using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public interface IBlockInfo
    {
        public GameColor Color { get; }
        public bool IsExited { get; }
        public IReadOnlyList<Vector2Int> OccupiedOffsets { get; }
        public bool OnCompletelyDestroyed { get; }
        public bool IsSatifiedConditions(IGateInfo gateInfo);
        public event Action OnExited;
    }

}