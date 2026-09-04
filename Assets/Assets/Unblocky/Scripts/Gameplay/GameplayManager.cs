using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public class GameplayManager : BaseMono
    {
        public static GameplayManager Instance { get; private set; }

        [SerializeField] private List<IBlockController> _activeBlocks;

        [SerializeField] private GameplayType _gameplayType;

        public event Action OnSetup;
        public event Action OnPlaying;
        public event Action OnPause;
        public event Action OnWin;
        public event Action OnLose;

        private LevelSystem _levelSystem;

        public GameplayType GameplayType => _gameplayType;

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
        }

        public override void Initialize()
        {
            base.Initialize();

            if (ServiceLocator.TryGet<LevelSystem>(out var levelSystem) == false) return;
            _levelSystem = levelSystem;
        }

        public void AddBlock(IBlockController blockController)
        {
            _activeBlocks.Add(blockController);
            blockController.OnControllerExited += RemoveAndCheckWin;
        }

        private void RemoveAndCheckWin(IBlockController deadController)
        {
            // 1. Thu hồi bộ đàm để chống Rò rỉ bộ nhớ
            deadController.OnControllerExited -= RemoveAndCheckWin;

            // 2. Gạch tên khỏi sổ Nam Tào (Giải phóng RAM)
            _activeBlocks.Remove(deadController);
            // 3. Tốc độ ánh sáng O(1): Kiểm tra xem sổ Nam Tào còn ai không?
            if (_activeBlocks.Count == 0)
            {
                Debug.Log("SẠCH BÀN CỜ! THẮNG RỒI!");
                SetGameType(GameplayType.Win);
            }
        }

        public void SetGameType(GameplayType newGameType)
        {
            _gameplayType = newGameType;
            OnGameTypeChanged();
        }

        public void OnGameTypeChanged()
        {
            switch (_gameplayType)
            {
                case GameplayType.Setup:
                    OnSetup?.Invoke();
                    break;
                case GameplayType.Playing:
                    OnPlaying?.Invoke();
                    break;
                case GameplayType.Pause:
                    OnPause?.Invoke();
                    break;
                case GameplayType.Win:
                    OnWin?.Invoke();
                    break;
                case GameplayType.Lose:
                    OnLose?.Invoke();
                    break;
                default:
                    Debug.LogWarning("Unknown Gameplay Type");
                    break;
            }
        }

    }
}