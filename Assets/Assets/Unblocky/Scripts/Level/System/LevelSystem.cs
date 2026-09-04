using System;
using UnityEngine;

namespace Flavor
{
    public class LevelSystem : BaseSystem, ISaveable
    {
        [SerializeField]private LevelConfigSO _levelConfigSO;
        public LevelConfigSO GetLevelConfig => _levelConfigSO;
        public string SaveID => throw new NotImplementedException();

        public Type SaveDataType => throw new NotImplementedException();

        public object CaptureSaving()
        {
            throw new NotImplementedException();
        }

        public void RestoreSaving(object data)
        {
            throw new NotImplementedException();
        }
    }

    public class LevelSaveData : ISaveData
    {

    }
}