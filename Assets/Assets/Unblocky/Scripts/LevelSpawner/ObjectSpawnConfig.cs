using System;
using UnityEngine;

namespace Flavor
{
    [Serializable]
    public class ObjectSpawnConfig
    {
        public int x;
        public int y;
        public string PrefabName;

        public ObjectSpawnConfig(int x, int y, string prefabName)
        {
            this.x = x;
            this.y = y;
            this.PrefabName = prefabName;
        }
    }
}