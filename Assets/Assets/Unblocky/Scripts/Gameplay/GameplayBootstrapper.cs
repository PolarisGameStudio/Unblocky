using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Flavor
{
    public class GameplayBootstrapper : BaseMono
    {
        public static bool IsLoaded = false;
        [SerializeField] private LevelSpawner _levelSpawner;
        [SerializeField] private GameplayController _gameplayController;

        public override void Initialize()
        {
            base.Initialize();
            SetupScene();
        }

        public async void SetupScene()
        {
            if (ServiceLocator.TryGet<LevelSystem>(out LevelSystem levelSystem) == false) return;


            _levelSpawner.SetData(levelSystem);
            await _levelSpawner.SpawnLevel();

            _gameplayController.SetData(levelSystem.GetLevelConfig);

            IsLoaded = true;

            await UniTask.Delay(3);
            _gameplayController.SetupGame();

        }


    }
}