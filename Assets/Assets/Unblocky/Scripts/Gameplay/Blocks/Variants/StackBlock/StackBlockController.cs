using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Flavor
{
    public class StackBlockController : BaseMono, IBlockController
    {
        [SerializeField] private IBlockVisual _stackBlockVisual;
        [SerializeField] private StackBlockBehavior _stackBlockBehavior;
        [SerializeField] private IBlockExitBehavior _stackBlockExitGateBehavior;
        [SerializeField] private DraggableObject _draggableObject;
        [SerializeField] private PlaceableObject _placeableObject;

        private List<IBlockStateListener> _stateListeners;
        private List<IStackStateListener> _stackListeners;

        private BlockContext _ctx;

        public event Action<IBlockController> OnControllerExited;

        public IBlockInfo BlockInfo => _stackBlockBehavior;

        protected override void Awake()
        {
            base.Awake();
            if (_stackBlockBehavior == null)
                _stackBlockBehavior = GetComponent<StackBlockBehavior>();

            if (_stackBlockVisual == null)
                _stackBlockVisual = GetComponent<IBlockVisual>();

            if (_draggableObject == null)
                _draggableObject = GetComponent<DraggableObject>();

            if (_placeableObject == null)
                _placeableObject = GetComponent<PlaceableObject>();

            if (_stackBlockExitGateBehavior == null)
                _stackBlockExitGateBehavior = GetComponent<IBlockExitBehavior>();

            _ctx = new BlockContext
            {
                BlockController = this,
                DraggableObject = _draggableObject,
                PlaceableObject = _placeableObject,
            };

            _stateListeners = GetComponents<IBlockStateListener>().ToList();
            _stackListeners = GetComponents<IStackStateListener>().ToList();
        }

        public override void Initialize()
        {
            base.Initialize();
            GameplayManager.Instance.AddBlock(this);
            _stackBlockExitGateBehavior.Init(_ctx);
        }

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            _stackBlockBehavior.OnStackUpdated += BroadcastStackUpdated;
            _draggableObject.OnDragStarted += BroadcastBeginDrag;
            _draggableObject.OnDragEnded += BroadcastEndDrag;
            _stackBlockBehavior.OnExited += HandleLogicExited;
        }

        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            _stackBlockBehavior.OnStackUpdated -= BroadcastStackUpdated;
            _draggableObject.OnDragStarted -= BroadcastBeginDrag;
            _draggableObject.OnDragEnded -= BroadcastEndDrag;
            _stackBlockBehavior.OnExited -= HandleLogicExited;
        }

        private void BroadcastStackUpdated(int index, GameColor color)
        {
            // Sếp Bắc loa
            _stackListeners.ForEach(l => l.OnStackUpdated(index, color));

            GameObject activeObj = _stackBlockVisual.GameObject;
            if (activeObj != null)
            {
                // 2. TỰ TAY SẾP MÓC LOGIC TỪ TRONG GAMEOBJECT RA!
                var newConditions = activeObj.GetComponents<IBlockFitCondition>().ToList();

                // 3. Đưa cho Não bộ (Behavior)
                _stackBlockBehavior.SetCurrentConditions(newConditions);
            }
        }
        private void BroadcastBeginDrag() => _stateListeners.ForEach(l => l.OnBeginDrag());
        private void BroadcastEndDrag() => _stateListeners.ForEach(l => l.OnEndDrag());

        public void OnExitedGate(IGateInfo gateInfo, Action onComplete = null)
        {
            // 4. BẮT ĐẦU BAY (1 giây sau nó mới tự động mở cái hộp onAnimationFinished ra chạy)
            _stackBlockExitGateBehavior.Execute(gateInfo, onComplete);
        }


        public async UniTask SetupData(BlockSetupInfo info)
        {
            await _stackBlockVisual.SetupVisual(info);
            await _stackBlockBehavior.SetupData(info, _placeableObject);
        }

        private void HandleLogicExited()
        {
            if (_stackBlockBehavior.OnCompletelyDestroyed)
                OnControllerExited?.Invoke(this);
        }
    }
}