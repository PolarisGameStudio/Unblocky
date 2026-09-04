using Cysharp.Threading.Tasks;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
namespace Flavor
{
    public class StackBlockBehavior : BaseMono, IBlockInfo, IDragState, IBlockStateListener
    {
        [SerializeField] private StackBlockModel _stackBlockModel;
        public event Action<int, GameColor> OnStackUpdated;
        public StackItemConfig NextItem => GetNextItem();

        public event Action OnExited;

        private List<IBlockFitCondition> _conditions;

        [SerializeField] private int _currentIndex;
        public int CurrentIndex => _currentIndex;
        public GameColor Color => GetCurrentItem()?.Config.Color ?? GameColor.None;
        public IReadOnlyList<Vector2Int> OccupiedOffsets => _stackBlockModel.Config.OccupiedOffset;

        public bool IsExited => GetCurrentItem()?.Runtime.IsExited ?? true; // Nếu hết gạch (null) thì coi như đã Exit xong
        public bool IsDragging { get; set; }
        public bool OnCompletelyDestroyed => _stackBlockModel.Runtime.IsExited;

        public StackItemModel GetCurrentItem()
        {
            if (_stackBlockModel == null || _stackBlockModel.Config == null || _stackBlockModel.Config.Items == null || _currentIndex < 0 || _currentIndex >= _stackBlockModel.Config.Items.Count)
                return null;

            return _stackBlockModel.ItemModels[_currentIndex];
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public StackItemConfig GetNextItem()
        {
            if (_stackBlockModel == null || _stackBlockModel.Config == null || _stackBlockModel.Config.Items == null) return null;
            var nextIndex = _currentIndex;
            nextIndex++;
            if (nextIndex >= _stackBlockModel.Config.Items.Count) return null;
            return _stackBlockModel.Config.Items[nextIndex];
        }

        public void PlusIndex()
        {
            _currentIndex++;
            if (_currentIndex >= _stackBlockModel.Config.Items.Count) return;
            OnStackUpdated?.Invoke(_currentIndex, GetNextItem()?.Color ?? GameColor.None);
        }

        public void OnBlockChildExited()
        {
            GetCurrentItem().Runtime.IsExited = true;
            OnExited?.Invoke();
        }

        public void OnBlockExited()
        {
            _stackBlockModel.Runtime.IsExited = true;
            OnExited?.Invoke();
        }


        public UniTask SetupData(BlockSetupInfo info, PlaceableObject placeable)
        {
            // LƯỢC DỊCH DATA (Mapping)
            List<StackItemConfig> itemsConfig = new List<StackItemConfig>();
            // Nếu có data từ Editor thì mới bắt đầu dịch
            if (info.StackItems != null)
            {
                itemsConfig = info.StackItems.Select(saveData => new StackItemConfig
                {
                    Color = saveData.Color,
                }).ToList();
            }



            var stackBlockConfig = new StackBlockConfig
            {
                Direction = info.Direction,
                Items = itemsConfig, // Gán vô tư vì đã cùng kiểu dữ liệu!
                OccupiedOffset = placeable.OccupiedOffsets
            };

            var stackBlockRuntime = new StackBlockRuntime();
            stackBlockRuntime.Items = new List<StackItemRuntime>(); // Phải khởi tạo List trước khi Add!
            
            foreach (var item in itemsConfig)
            {
                stackBlockRuntime.Items.Add(new StackItemRuntime());
            }


            _stackBlockModel = new(stackBlockConfig, stackBlockRuntime);

            OnStackUpdated?.Invoke(_currentIndex, GetNextItem()?.Color ?? GameColor.None);

            return UniTask.CompletedTask;
        }

        public bool IsSatifiedConditions(IGateInfo info)
        {
            foreach (var condition in _conditions)
            {
                if (condition.IsMatch(info) == false) return false;
            }
            return true;
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

        public void SetCurrentConditions(List<IBlockFitCondition> newConditions)
        {
            _conditions = newConditions; // Não bộ cập nhật danh sách luật mới

            var blockContext = new BlockFitConditionContext
            {
                Color = this.Color,
                Direction = _stackBlockModel.Config.Direction
            };

            this.Log($"[Debug] Cập nhật Context mới -> Màu: {this.Color} | Luật mới: {_conditions.Count} cái");
            foreach (var condition in _conditions)
            {
                condition.Init(blockContext);
            }
        }
    }
}
