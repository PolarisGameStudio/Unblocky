using UnityEngine;

namespace Flavor
{
    public class GameSystem : BaseSystem
    {
        public GameColorConfig currentColorConfig;

        private void Awake()
        {
          
        }

        public override void Initialize()
        {
            base.Initialize();

            ServiceLocator.Register(currentColorConfig.GetType(), currentColorConfig);

            IsInitialized = true;
        }
    }
}