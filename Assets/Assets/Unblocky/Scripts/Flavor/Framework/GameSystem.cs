using UnityEngine;

namespace Flavor
{
    public class GameSystem : BaseSystem
    {
        public static GameSystem Instance { get; private set; }
        private void Awake()
        {
            var a = GetComponent<MainApplication>();
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            IsInitialized = true;
        }
    }
}