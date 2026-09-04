using UnityEngine;

namespace Flavor
{
    public class MainMenuBootstrapper : BaseMono
    {
        public static bool IsLoaded = false;

        public override void Initialize()
        {
            base.Initialize();
            SetupScene();
        }

        public async void SetupScene()
        {
            IsLoaded = true;
        }

    }
}