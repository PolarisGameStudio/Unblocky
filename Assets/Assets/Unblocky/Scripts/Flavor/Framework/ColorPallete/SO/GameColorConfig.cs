using UnityEngine;
using System.Collections.Generic;

namespace Flavor
{
    // Dòng này giúp bạn r-click tạo file config ngay trong Project window
    [CreateAssetMenu(fileName = "GameColorConfig", menuName = "Flavor/Color Config")]
    public class GameColorConfig : ScriptableObject
    {
        // Có thể dùng List hoặc Dictionary, nhưng dùng struct/class nhỏ là rõ ràng nhất
        [Header("Block Colors")]
        [SerializeField] private Color _red = Color.red;
        [SerializeField] private Color _blue = Color.blue;
        [SerializeField] private Color _yellow = Color.yellow;
        [SerializeField] private Color _pink = new Color32(255, 105, 180, 255);

        // Hoặc xịn hơn, làm một hàm lấy màu tự động theo Enum:
        public Color GetColor(GameColor type)
        {
            switch (type)
            {
                case GameColor.Red: return _red;
                case GameColor.Blue: return _blue;
                case GameColor.Yellow: return _yellow;
                case GameColor.Pink: return _pink;
                default: return Color.white;
            }
        }
    }
}
