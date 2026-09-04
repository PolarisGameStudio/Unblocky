using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Flavor
{
    public class GateController : BaseMono, IGateController
    {
        [SerializeField] private GateVisual _gateVisual;
        [SerializeField] private GateDetective _gateDetective;
        [SerializeField] private GateExitHandler _gateExitHandler;
        [SerializeField] private GateBehavior _gateBehavior;
        [SerializeField] private GatePassHandler _gatePassHandler;

        public IGateInfo GateInfo => _gateBehavior;

        protected override void Awake()
        {
            base.Awake();
            if (_gateVisual == null)
                _gateVisual = GetComponent<GateVisual>();
            if (_gateBehavior == null)
                _gateBehavior = GetComponent<GateBehavior>();
            if (_gateDetective == null)
                _gateDetective = GetComponent<GateDetective>();
            if (_gateExitHandler == null)
                _gateExitHandler = GetComponent<GateExitHandler>();
            if(_gatePassHandler == null)
                _gatePassHandler = GetComponent<GatePassHandler>();
        }

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            _gateDetective.OnDetectBlock += HandleBlockDetected; // Truyền thẳng Lệnh cho Quan tòa

            _gateExitHandler.OnBlockExitSuccess += _gatePassHandler.HandleBlockPass; // Truyền lệnh cho Gác Cửa

            _gatePassHandler.OnOpenGate += _gateVisual.PlayEffect;
            _gatePassHandler.OnCloseGate += _gateVisual.StopEffect;
        }


        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            _gateDetective.OnDetectBlock -= HandleBlockDetected;

            _gateExitHandler.OnBlockExitSuccess -= _gatePassHandler.HandleBlockPass;

            _gatePassHandler.OnOpenGate -= _gateVisual.PlayEffect;
            _gatePassHandler.OnCloseGate -= _gateVisual.StopEffect;
        }

        // Tạo hàm trung gian này nằm ngay trong class (Dưới hàm UnlisteningEvents)
        private void HandleBlockDetected(IBlockController detectedBlock)
        {
            // Hàm này sẽ tự động chạy khi GateDetective phát hiện gạch
            // Và nó sẽ truyền con gạch + chính bản thân cái Cửa (this) cho Quan Tòa xử lý
            _gateExitHandler.TryToExit(detectedBlock, this);
        }

        public async UniTask SetupData(GateSetupInfo info)
        {
            await _gateBehavior.SetupData(info);
            await _gateVisual.SetupData(info);
        }
    }
}