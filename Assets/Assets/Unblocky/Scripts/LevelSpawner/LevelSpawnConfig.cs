using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    [Serializable]
    public class LevelSpawnConfig
    {
        public int LevelIndex { get; set; }
        public int Width;
        public int Height;
        public List<ObjectSpawnConfig> ObjectsSpawnConfig = new List<ObjectSpawnConfig>();
    }
}