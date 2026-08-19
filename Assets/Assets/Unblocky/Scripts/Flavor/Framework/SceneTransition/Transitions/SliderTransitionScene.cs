using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flavor
{
    public class SliderTransitionScene : BaseTransitionScene
    {
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TextMeshProUGUI _progressText; // Tùy chọn nếu có text %

        public override IEnumerator TransitionIn()
        {
            // Reset thanh loading về 0 khi bắt đầu chuyển cảnh
            if (_loadingSlider != null) _loadingSlider.value = 0f;
            if (_progressText != null) _progressText.text = "0%";
            yield break; // Không cần chờ gì cả
        }

        // Cập nhật giá trị khi Scene đang được nạp ngầm
        public override void SetProgress(float progress)
        {
            if (_loadingSlider != null)
                _loadingSlider.value = progress;

            if (_progressText != null)
                _progressText.text = $"{(int)(progress * 100)}%";
        }

        public override IEnumerator TransitionOut()
        {
            yield break;
        }
    }
}
