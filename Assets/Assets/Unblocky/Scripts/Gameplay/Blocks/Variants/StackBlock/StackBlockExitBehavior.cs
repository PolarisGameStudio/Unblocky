using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Flavor
{
    public class StackBlockExitGateBehavior : BaseMono, IBlockExitBehavior
    {
        [SerializeField] private StackBlockBehavior _stackBlockBehavior;
        private BlockContext _ctx;

        // Yêu cầu Inspector kéo thả đống StateListeners vào đây hoặc tự GetComponent
        private List<IBlockStateListener> _stateListeners;
        protected override void Awake()
        {
            base.Awake();
            _stateListeners = GetComponents<IBlockStateListener>().ToList();

            if (_stackBlockBehavior == null)
                _stackBlockBehavior = GetComponent<StackBlockBehavior>();
        }

        public void Init(BlockContext ctx)
        {
            _ctx = ctx;
        }

        public void Execute(IGateInfo gateInfo, Action onComplete = null)
        {
            if (ServiceLocator.TryGet<BoardSystem>(out var bm) == false) return;
            _stackBlockBehavior.OnBlockChildExited();
            _ctx.DraggableObject.OnDragCancel();

            var currentIndex = _stackBlockBehavior.CurrentIndex;
            var recordIndex = currentIndex;

            this.Log($"[RecordIndex] {recordIndex}");

            bool isLastItem = _stackBlockBehavior.NextItem == null;

            if (isLastItem)
            {
                // Chỉ UnregisterGrid thôi, KHÔNG ĐƯỢC TẮT GAMEOBJECT Ở ĐÂY!
                _ctx.PlaceableObject.RequestRemoval();
                _stackBlockBehavior.OnBlockExited();
            }

            _stackBlockBehavior.PlusIndex();

            Vector2Int vGrid = Vector2Int.zero;
            vGrid = bm.TranslateWorldToGrid(transform.position);

            PlacementController.Instance.PlaceInstant(_ctx.PlaceableObject, vGrid);

            Action onAnimationFinished = () =>
            {
                onComplete?.Invoke();

                // 3. KIỂM TRA TRÍ NHỚ: Bay xong rồi, nếu nãy là thằng cuối thì tắt Game Object!
                if (isLastItem)
                    this.gameObject.SetActive(false);
            };

            var blockSize = _ctx.PlaceableObject.GetMaxSize();
            var vDirection = gateInfo.Direction.ToWorldVector();
            int exitDistance = gateInfo.Direction.GetExitDistance(blockSize);
            int multiplier = exitDistance > 0 ? exitDistance : 1;
            Vector3 vTarget = transform.position + (vDirection * multiplier);

            this.LogWarning($"_stackBlockBehavior.CurrentIndex blockSize {blockSize} vDirection {vDirection} multipler {multiplier} vTarget {vTarget}");
            this.Log($"[RecordIndex] {recordIndex}");
            var exitAnimationData = new StackBlockExitData
            {
                IsLast = isLastItem,
                StackIndex = recordIndex, // Fix lại cho đúng index vừa thoát
                TargetPosition = vTarget,
                OnComplete = onAnimationFinished,
                GateInfo = gateInfo,
            };
            // 4. Báo cho tụi Visual biết để bay ra
            _stateListeners.ForEach(l => l.OnExitedGate(exitAnimationData));
        }
    }

}