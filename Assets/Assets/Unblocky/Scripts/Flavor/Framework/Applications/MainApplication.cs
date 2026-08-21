using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace Flavor
{
    public class MainApplication : MonoBehaviour
    {
        public static MainApplication Instance { get; private set; }
        private TickService _tickService;

        [Header("Systems")]
        [SerializeField] private SystemConfigSO _systemConfig;
        [SerializeField] private List<BaseSystem> _systems;
        public bool IsInitialized { get; protected set; }

        #region Unity Methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private async void Start()
        {
            Initialize();
            await LoadSystems();

            IsInitialized = true;
        }


        // Nhận event từ Unity và truyền vào TickService
        private void Update()
        {
            // Update đại diện cho cả EarlyTick và Tick. 
            // Bạn gọi EarlyTick trước, sau đó mới tới Tick.
            _tickService?.EarlyTick();
            _tickService?.Tick();
        }
        private void FixedUpdate()
        {
            _tickService?.FixedTick();
        }
        private void LateUpdate()
        {
            _tickService?.LateTick();
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        #region Tick Methods
        public void RegisterTickable(ITickable tickable, TickMode mode)
        {
            _tickService?.Register(tickable, mode);
        }

        public void UnregisterTickable(ITickable tickable)
        {
            _tickService?.Unregister(tickable);
        }

        #endregion

        #region LifeCycle Methods
        public void Initialize()
        {
            _tickService = new TickService();
        }

        public void Dispose()
        {

        }
        #endregion

        private async UniTask LoadSystems()
        {
            foreach (var system in _systemConfig.Systems)
            {
                try
                {
                    GameObject spawnedObj = await system.InstantiateAsync().Task;
                    spawnedObj.transform.SetParent(this.transform);

                    BaseSystem mySystem = spawnedObj.GetComponent<BaseSystem>();

                    if (mySystem != null)
                    {
                        _systems.Add(mySystem);
                        Debug.LogWarning($"[BaseSystem] Đã Spawn: {system.GetType()} - {system}");
                    }

                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[BaseSystem] Failed to load system: {system.RuntimeKey} | {e.Message}");
                }
            }

            var sortedSystems = _systems.OrderBy(s => s.Priority).ToList();

            foreach (var system in sortedSystems)
            {
                if (system == null) continue;

                // 1. MainApplication đứng ra đăng ký System này vào ServiceLocator theo đúng Type của nó
                ServiceLocator.Register(system.GetType(), system);
                // 2. Gọi hàm khởi tạo cho System
                system.Initialize();

                await system.LoadDataAsync();

            }

            Debug.Log("[MainApplication] HOÀN TẤT: Toàn bộ hệ thống đã lên mâm!");
            this.LogWarning("HOÀN TẤT: Toàn bộ hệ thống đã lên mâm!");
        }
    }

    public static class ApplicationExtensions
    {
        public static MainApplication GetApplication(this MonoBehaviour mono)
        {
            return MainApplication.Instance;
        }
    }
}