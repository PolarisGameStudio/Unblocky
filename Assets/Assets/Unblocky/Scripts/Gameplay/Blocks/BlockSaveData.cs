using System;
using System.Collections.Generic;
using UnityEngine;
namespace Flavor
{
    [Serializable]
    public class BlockSaveData
    {
        public string BlockName;
        public BlockType Type;
        public Vector2Int GridSpawn;

        public GameColor Color;

        public DirectionType Direction;

        public List<StackItemSaveData> StackItems;
    }
}