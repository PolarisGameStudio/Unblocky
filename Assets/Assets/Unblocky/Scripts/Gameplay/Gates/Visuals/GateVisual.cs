using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Flavor
{
    public class GateVisual : BaseMono
    {
        private MeshRenderer _mesh;
        private GateAnim _anim;

        private static MaterialPropertyBlock _propBlock;
        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");


        protected override void Awake()
        {
            base.Awake();
            if(_anim == null)
                _anim = GetComponent<GateAnim>();
            if(_mesh == null)
                _mesh = GetComponentInChildren<MeshRenderer>();

            _propBlock = new MaterialPropertyBlock();
        }

        public UniTask SetupData(GateSetupInfo info)
        {
            SetColorMaterial(info.RequiredColor);
            return UniTask.CompletedTask;
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

        public void PlayEffect()
        {
            _anim.PlayAnim(AnimHash.OPENHASH); // Mở cổng
            _anim.StopAnim(AnimHash.CLOSEHASH);
        }

        public void StopEffect()
        {
            _anim.PlayAnim(AnimHash.CLOSEHASH);
            _anim.StopAnim(AnimHash.OPENHASH);
        }

        // có function set Visual(GameColor _color);
    }
}
