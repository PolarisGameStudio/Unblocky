using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    [Serializable]
    public class StackBlockConfig
    {
        public IReadOnlyList<Vector2Int> OccupiedOffset; // Ví d? c?t này chi?u xu?ng ??t là 1x1
        public DirectionType Direction;         // C?t này ???c tr??t h??ng nào
        public List<StackItemConfig> Items;
    }
}