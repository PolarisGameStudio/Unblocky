using DG.Tweening;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Flavor
{
    public class BlockExitBehavior : BaseMono, IBlockExitBehavior
    {
        private BlockBehavior _blockBehavior;
        private List<IBlockStateListener> _listeners;
        private BlockContext _ctx;

        protected override void Awake()
        {
            base.Awake();
            // Quét Loa phường 1 lần duy nhất
            _listeners = GetComponents<IBlockStateListener>().ToList();
            _blockBehavior = GetComponent<BlockBehavior>();
        }

        public void Init(BlockContext ctx)
        {
            _ctx = ctx;
        }

        public void Execute(IGateInfo gateInfo, Action onComplete = null)
        {
            if (ServiceLocator.TryGet<BoardSystem>(out var bm) == false) return;
            _ctx.DraggableObject.OnDragCancel();

            _ctx.PlaceableObject.RequestRemoval();

            _blockBehavior.OnMarkExited();

            Vector2Int vGrid = Vector2Int.zero;
            vGrid = bm.TranslateWorldToGrid(transform.position);

            PlacementController.Instance.PlaceInstant(_ctx.PlaceableObject, vGrid);

            var blockSize = _ctx.PlaceableObject.GetMaxSize();

            // LÀM TOÁN BẰNG DATA ĐƯỢC CẤP
            var vDirection = gateInfo.Direction.ToWorldVector();
            int exitDistance = gateInfo.Direction.GetExitDistance(blockSize); // Dùng Size được cấp
            int multiplier = exitDistance > 0 ? exitDistance : 1;
            Vector3 vTarget = transform.position + (vDirection * multiplier);

            Action onAnimationFinished = () =>
            {
                onComplete?.Invoke();
                
            };

            // HÔ TO VÀO LOA
            BaseBlockExitData exitData = new BaseBlockExitData
            {
                TargetPosition = vTarget,
                OnComplete = onComplete,
                GateInfo = gateInfo,
            };
            _listeners.ForEach(l => l.OnExitedGate(exitData));
        }

    }
}