using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Color = UnityEngine.Color;

namespace Flavor
{
    public class StackBlockVisual : BaseMono, IBlockVisual, IStackStateListener, IBlockStateListener
    {
        [SerializeField] private List<GameObject> _spawnedVisuals;
        [SerializeField] private MeshRenderer _pendMesh;
        [SerializeField] private int _currentIndex = 0;

        private static MaterialPropertyBlock _propBlock;
        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

        public GameObject GameObject => _spawnedVisuals[_currentIndex];

        protected override void Awake()
        {
            base.Awake();

            // 2. Khởi tạo thẻ tên (chỉ chạy 1 lần cho mọi cục gạch)
            if (_propBlock == null)
                _propBlock = new MaterialPropertyBlock();
        }

        public async UniTask SpawnVisuals(BlockSetupInfo config)
        {
            for (int i = 0; i < config.StackItems.Count; i++)
            {
                var itemConfig = config.StackItems[i];
                var itemName = itemConfig.BlockName;
                GameObject itemObj = await AddressablesExtensions.InstantiateAsync(itemName);
                if (itemObj == null) continue;

                itemObj.transform.SetParent(this.transform);
                itemObj.transform.position = transform.position;
                itemObj.gameObject.SetActive(false);

                if (itemObj.TryGetComponent<BlockVisual>(out var childVisual))
                {
                    childVisual.SetColorMaterial(itemConfig.Color);
                }

                _spawnedVisuals.Add(itemObj);

            }

            foreach (var obj in _spawnedVisuals)
            {
                if (obj == null) continue;
                obj.SetActive(false);
            }
        }

        public void ShowBlockAt(int index, GameColor pendingColor)
        {
            _currentIndex = index;


            if (index < _spawnedVisuals.Count)
            {
                _spawnedVisuals[index].SetActive(true);
            }

            // 2. X? lý C?c Pending (??i màu b? ??)
            if (pendingColor == GameColor.None)
            {
                // H?t g?ch r?i, t?t luôn cái b? ?? ?i cho ??p
                _pendMesh.gameObject.SetActive(false);
            }
            else
            {
                _pendMesh.gameObject.SetActive(true);
                if (ServiceLocator.TryGet<GameSystem>(out var gameSystem) == false) return;
                // 4. DÙNG PROPERTY BLOCK ĐỂ ĐỔI MÀU BỆ ĐỠ (Tối ưu GPU Instancing)
                Color realColor = gameSystem.ColorConfig.GetColor(pendingColor);

                _pendMesh.GetPropertyBlock(_propBlock);
                _propBlock.SetColor(BaseColorID, realColor);
                _pendMesh.SetPropertyBlock(_propBlock);
            }
        }

        public async UniTask SetupVisual(BlockSetupInfo info)
        {
            await SpawnVisuals(info);
        }

        public void OnStackUpdated(int index, GameColor color)
        {
            this.Log($"OnStackUpdated index {index} Color {color}");
            ShowBlockAt(index, color);
        }

        public void OnBeginDrag()
        {
            if (_currentIndex < _spawnedVisuals.Count)
            {
                var activeChild = _spawnedVisuals[_currentIndex];
                List<IBlockStateListener> childListeners = new();
                if (activeChild == null) return;

                childListeners = activeChild.GetComponents<IBlockStateListener>().ToList();

                foreach (var child in childListeners)
                {
                    child.OnBeginDrag();
                }
            }

        }

        public void OnEndDrag()
        {
            if (_currentIndex < _spawnedVisuals.Count)
            {
                var activeChild = _spawnedVisuals[_currentIndex];
                List<IBlockStateListener> childListeners = new();
                if (activeChild == null) return;

                childListeners = activeChild.GetComponents<IBlockStateListener>().ToList();

                foreach (var child in childListeners)
                {
                    child.OnEndDrag();
                }
            }
        }

        public void OnExitedGate(BaseBlockExitData exitData)
        {
            OnEndDrag();

            var stackExitData = exitData as StackBlockExitData;

            if (stackExitData == null) return;

            var blockVisualObj = _spawnedVisuals[stackExitData.StackIndex];

            if (blockVisualObj == null) return;

            var activeChild = _spawnedVisuals[stackExitData.StackIndex];
            List<IBlockStateListener> childListeners = new();
            if (activeChild == null) return;

            childListeners = activeChild.GetComponents<IBlockStateListener>().ToList();
            blockVisualObj.transform.SetParent(null);
            foreach (var child in childListeners)
            {
                child.OnExitedGate(exitData);
            }
        }

    }

}