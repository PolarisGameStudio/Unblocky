using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Flavor
{
    public class BlockDirectionVisual : BaseMono, IBlockVisual
    {
        [SerializeField] private Transform _arrowTransform; // Kéo object Arrow vào đây

        public GameObject GameObject => this.gameObject;

        public void UpdateArrowRotation(DirectionType direction)
        {
            // Tùy theo logic cờ (Flag) của bạn, đây là ví dụ:
            if (direction == DirectionType.Vertical)
            {
                _arrowTransform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else if (direction == DirectionType.Horizontal)
            {
                _arrowTransform.localEulerAngles = Vector3.zero;
            }
            // Thêm các case Top, Bot... tùy ý bạn
        }

        public UniTask SetupVisual(BlockSetupInfo info)
        {
            UpdateArrowRotation(info.Direction);
            return UniTask.CompletedTask;
        }


    }
}