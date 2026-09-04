using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Flavor
{
    public class BlockController : BaseMono, IBlockController
    {
        [SerializeField] private BlockBehavior _blockBehavior;
        [SerializeField] private IBlockExitBehavior _exitBehavior;
        [SerializeField] private List<IBlockVisual> _blockVisuals;

        [SerializeField] private DraggableObject _draggableObject;
        [SerializeField] private PlaceableObject _placeableObject;
        private List<IBlockStateListener> _stateListeners;

        private BlockContext _ctx;

        public event Action<IBlockController> OnControllerExited;

        public IBlockInfo BlockInfo => _blockBehavior;


        protected override void Awake()
        {
            base.Awake();
            if (_blockBehavior == null)
                _blockBehavior = GetComponent<BlockBehavior>();
            if (_blockVisuals == null)
                _blockVisuals = GetComponents<IBlockVisual>().ToList();
            if (_exitBehavior == null)
                _exitBehavior = GetComponent<IBlockExitBehavior>();

            if (_draggableObject == null)
                _draggableObject = GetComponent<DraggableObject>();
            if (_placeableObject == null)
                _placeableObject = GetComponent<PlaceableObject>();

            _ctx = new BlockContext
            {
                BlockController = this,
                DraggableObject = _draggableObject,
                PlaceableObject = _placeableObject,
            };

            _stateListeners = GetComponents<IBlockStateListener>().ToList();
        }

        private void BroadcastBeginDrag() => _stateListeners.ForEach(l => l.OnBeginDrag());
        private void BroadcastEndDrag() => _stateListeners.ForEach(l => l.OnEndDrag());

        public override void Initialize()
        {
            base.Initialize();
            GameplayManager.Instance.AddBlock(this);
            _exitBehavior.Init(_ctx);
        }

        public async UniTask SetupData(BlockSetupInfo info)
        {
            await _blockBehavior.SetupData(info, _placeableObject);
            foreach (var blockVisual in _blockVisuals)
            {
                await blockVisual.SetupVisual(info);
            }
        }

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            _draggableObject.OnDragStarted += BroadcastBeginDrag;
            _draggableObject.OnDragEnded += BroadcastEndDrag;
            _blockBehavior.OnExited += HandleLogicExited;
        }

        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            _draggableObject.OnDragStarted -= BroadcastBeginDrag;
            _draggableObject.OnDragEnded -= BroadcastEndDrag;
            _blockBehavior.OnExited -= HandleLogicExited;
        }


        public void OnExitedGate(IGateInfo gateInfo, Action onComplete = null)
        {
            _exitBehavior.Execute(gateInfo, onComplete);
        }

        private void HandleLogicExited()
        {
            OnControllerExited?.Invoke(this);
        }

    }
}

