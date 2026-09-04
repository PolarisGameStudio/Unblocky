using System;
using TMPro;
using UnityEngine;

namespace Flavor
{
    public class CountdownUI : BaseMono
    {
        public event Action<float> OnSecondChanged;
        private float _lastSecond = -1;
        // Manager bên ngoài chỉ cần gọi đúng hàm này
        public void UpdateTime(float remainingSeconds)
        {
            if (remainingSeconds == _lastSecond) return;
            _lastSecond = remainingSeconds;
            // Bắn tín hiệu cho bọn Đệ (Lego) tự làm việc
            OnSecondChanged?.Invoke(remainingSeconds);
        }
    }
}