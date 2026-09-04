using UnityEngine;

namespace Flavor
{
    public class GameplayController : BaseMono
    {
        public static GameplayController Instance { get; private set; }

        private GameplaySetupInfo _gameplayConfig;
        private CountdownMode _countdownMode;
        private bool _isGameStarted;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (_countdownMode == null)
                _countdownMode = GetComponent<CountdownMode>();
        }

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            _countdownMode.OnOutOfTime += HandleOutOfTime;
            GameplayEvent.OnStartPlaying += StartGame;

            var gameplayUI = ServiceLocator.Get<UISystem>().GetUI<GameplayUI>();

            // 2. Nối dây đồng hồ!
            _countdownMode.OnTimePassed += gameplayUI.CountdownUI.UpdateTime;
        }

        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            _countdownMode.OnOutOfTime -= HandleOutOfTime;

            var gameplayUI = ServiceLocator.Get<UISystem>().GetUI<GameplayUI>();
            _countdownMode.OnTimePassed -= gameplayUI.CountdownUI.UpdateTime;
        }

        public void SetData(LevelConfigSO levelConfig)
        {
            _gameplayConfig = levelConfig.GameplaySetupInfo;
            _countdownMode.SetCountdown(_gameplayConfig.SecondsCountdown);
        }

        public void SetupGame()
        {
            GameplayManager.Instance.SetGameType(GameplayType.Setup);
        }

        public void StartGame()
        {
            if (_isGameStarted) return;

            GameplayManager.Instance.SetGameType(GameplayType.Playing);
            _isGameStarted = true;

        }

        private void HandleOutOfTime()
        {
            GameplayManager.Instance.SetGameType(GameplayType.Lose); // Hết giờ thì báo Thua!
        }

    }
}