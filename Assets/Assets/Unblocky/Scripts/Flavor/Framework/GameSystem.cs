using System;
using UnityEngine;

namespace Flavor
{
    public class GameSystem : BaseSystem, ISaveable
    {
        [Header("GAME SAVE DATA")]
        [SerializeField] private GameSaveData _saveData = new();

        public GameColorConfig ColorConfig;
        [SerializeField] private string _saveID;
        public string SaveID => _saveID;

        public Type SaveDataType => typeof(GameSaveData);

        #region Unity Cycle

        protected override void Awake()
        {
            base.Awake();
        }



        #endregion

        public override void Initialize()
        {
            base.Initialize();

            ServiceLocator.Register(ColorConfig.GetType(), ColorConfig);

            if (ServiceLocator.TryGet<SaveSystem>(out var saveSys) == false) return;
            saveSys.RegisterSaveable(this);

            IsInitialized = true;
        }

        public object CaptureSaving()
        {
            return _saveData;
        }

        public void RestoreSaving(object data)
        {
            if (data != null)
            {
                _saveData = (GameSaveData)data;
            }
        }
    }

    [Serializable]
    public class GameSaveData : ISaveData
    {

    }
}