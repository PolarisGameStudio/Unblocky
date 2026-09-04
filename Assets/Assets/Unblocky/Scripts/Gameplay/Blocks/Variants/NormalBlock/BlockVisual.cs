using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.StickyNote;
using static UnityEngine.GraphicsBuffer;

namespace Flavor
{
    public class BlockVisual : BaseMono, IBlockVisual, IBlockStateListener
    {
        private MeshRenderer _mesh;
        private List<ClipToGate> _clipToGates;

        private Outline _outline;
        private static MaterialPropertyBlock _propBlock;
        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
        public GameObject GameObject => this.gameObject;

        protected override void Awake()
        {
            base.Awake();
            if (_outline == null)
                _outline = GetComponent<Outline>();
            if (_propBlock == null)
                _propBlock = new MaterialPropertyBlock();
            if (_mesh == null)
                _mesh = GetComponentInChildren<MeshRenderer>();

            _clipToGates = GetComponentsInChildren<ClipToGate>().ToList();
        }

        public override void Initialize()
        {
            base.Initialize();
            _outline.enabled = false;
        }

        public void ShowOutline()
        {
            _outline.enabled = true;
        }

        public void HideOutline()
        {
            _outline.enabled = false;
        }

        public void SetColorMaterial(GameColor color)
        {
            // 1. Dịch từ Enum (GameColor) sang Màu thật (Color)
            // Tùy cách bạn lấy Config, ở đây tôi ví dụ gọi qua ServiceLocator
            if (ServiceLocator.TryGet<GameSystem>(out var gameSystem))
            {
                Color realColor = gameSystem.ColorConfig.GetColor(color);
                // 2. Dán giấy đè màu lên Mesh mà không làm đúp Material
                _mesh.GetPropertyBlock(_propBlock);         // Lấy giấy hiện tại
                _propBlock.SetColor(BaseColorID, realColor); // Ghi đè màu mới
                _mesh.SetPropertyBlock(_propBlock);         // Dán lại vào cục gạch
            }
            else
            {
                Debug.LogError("Chưa khởi tạo gameSystem kìa!");
            }
        }

        public UniTask SetupVisual(BlockSetupInfo info)
        {
            SetColorMaterial(info.Color);
            return UniTask.CompletedTask;
        }

        public void OnBeginDrag()
        {
            ShowOutline();
        }

        public void OnEndDrag()
        {
            HideOutline();
        }

        public void OnExitedGate(BaseBlockExitData exitData)
        {
            this.Log($"[ExitedGate] Block Visual");
            HideOutline();

            var vTarget = exitData.TargetPosition;
            var onComplete = exitData.OnComplete;
            var gateInfo = exitData.GateInfo;

            foreach (var clip in _clipToGates)
            {
                clip.SetClippingGate(gateInfo);
            }

            transform.DOMove(vTarget, 1f).OnComplete(() =>
            {
                onComplete?.Invoke();
                this.gameObject.SetActive(false);
            });
        }

        // có function set Visual(GameColor _color);
    }

}