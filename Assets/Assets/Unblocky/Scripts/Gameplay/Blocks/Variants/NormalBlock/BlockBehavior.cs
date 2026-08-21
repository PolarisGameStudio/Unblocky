using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    class BlockBehavior : BaseMono, IBlockInfo
    {
        public BlockConfig Config;
        public List<IBlockFitCondition> conditions;

        public GameColor Color => Config.Color;
        public IReadOnlyList<Vector2Int> OccupiedOffsets => _placeableObject.OccupiedOffsets;

        public GameObject GameObject => this.GameObject;

        // 1. Tạo một biến private để lưu trữ (Cache)
        private PlaceableObject _placeableObject;

        private bool _isExited;

        private void Awake()
        {
            // 2. Chỉ gọi GetComponent đúng MỘT lần duy nhất khi game bắt đầu
            _placeableObject = GetComponent<PlaceableObject>();
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
        }
    }

}