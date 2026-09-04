using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Flavor
{
    public class BlockBehavior : BaseMono, IBlockInfo, IDragState, IBlockStateListener
    {
        public BlockConfig Config;
        public List<IBlockFitCondition> conditions;

        public GameColor Color => Config.Color;
        public IReadOnlyList<Vector2Int> OccupiedOffsets => _placeableObject.OccupiedOffsets;
        // 1. Tạo một biến private để lưu trữ (Cache)
        private PlaceableObject _placeableObject;

        private bool _isExited;
        public bool IsExited { get { return _isExited; } }

        public bool IsDragging { get; set; }

        public bool OnCompletelyDestroyed => _isExited;

        public event Action OnExited;

        protected override void Awake()
        {
            base.Awake();
            conditions = GetComponentsInChildren<IBlockFitCondition>().ToList();
        }

        public UniTask SetupData(BlockSetupInfo info, PlaceableObject placeable)
        {
            _placeableObject = placeable;

            var config = new BlockConfig { Color = info.Color };
            Config = config;


            var blockFitContext = new BlockFitConditionContext
            {
                Color = info.Color,
                Direction = info.Direction,
            };

            foreach (var condition in conditions)
            {
                condition.Init(blockFitContext);
            }

            return UniTask.CompletedTask;
        }

        public bool IsSatifiedConditions(IGateInfo gateInfo)
        {
            foreach (var condition in conditions)
            {
                if (condition.IsMatch(gateInfo) == false)
                {
                    return false;
                }

            }
            return true;

        }

        public void OnMarkExited()
        {
            _isExited = true;
            OnExited?.Invoke();
        }

        public void OnBeginDrag()
        {
            IsDragging = true;
        }

        public void OnEndDrag()
        {
            IsDragging = false;
        }

        public void OnExitedGate(BaseBlockExitData exitData)
        {
            IsDragging = false;
        }
    }

}