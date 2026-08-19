using System.Collections;
using UnityEngine;

namespace Flavor
{
    public class FadeTransitionScene : BaseTransitionScene
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 1f;
        private void Awake()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }
        /// <summary>
        /// Fade từ trong suốt (0) sang tối/che kín (1)
        /// </summary>
        public override IEnumerator TransitionIn()
        {
            _canvasGroup.blocksRaycasts = true; // Chặn bấm lung tung khi đang chuyển scene
            float elapsedTime = 0f;
            while (elapsedTime < _fadeDuration)
            {
                // Dùng unscaledDeltaTime để tránh bị đơ nếu game đang pause (Time.timeScale = 0)
                elapsedTime += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(elapsedTime / _fadeDuration);

                _canvasGroup.alpha = progress; // 0 -> 1
                yield return null;
            }
            _canvasGroup.alpha = 1f; // Đảm bảo chốt chắc chắn là 1
        }
        public override void SetProgress(float progress)
        {
            // Với Fade đơn giản thì không cần slider progress, để trống
        }
        /// <summary>
        /// Fade từ tối/che kín (1) sang trong suốt (0)
        /// </summary>
        public override IEnumerator TransitionOut()
        {
            float elapsedTime = 0f;
            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(elapsedTime / _fadeDuration);

                _canvasGroup.alpha = 1f - progress; // 1 -> 0
                yield return null;
            }
            _canvasGroup.alpha = 0f; // Đảm bảo chốt chắc chắn là 0
            _canvasGroup.blocksRaycasts = false; // Bỏ chặn tương tác UI
        }
    }
}