using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Flavor
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseUI : BaseMono
    {
        [Header("UI Settings")]
        [Tooltip("Khung chứa nội dung chính để chạy Animation thu phóng/trượt")]
        [SerializeField] protected RectTransform _contentPanel;

        [Tooltip("Lớp màn đen chặn click lót phía dưới (Có thể để trống)")]
        [SerializeField] protected Image _darkOverlay;

        [Header("Settings")]
        [SerializeField] protected float _animDuration = 0.3f;

        protected CanvasGroup _canvasGroup;
        protected bool _isInitialized = false;

        protected override void Awake()
        {
            base.Awake();
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
        }

        public async UniTask ShowAsync()
        {
            gameObject.SetActive(true);

            BeforeShow();          // 1. Chuẩn bị (Khóa click, Setup Alpha = 0)
            await PlayInAnim();    // 2. Chạy Hiệu Ứng
            AfterShow();           // 3. Xong xuôi (Mở khóa click)
        }

        public async UniTask HideAsync()
        {
            BeforeHide();          // 1. Chuẩn bị (Khóa click ngay lập tức)
            await PlayOutAnim();   // 2. Chạy Hiệu Ứng
            AfterHide();
        }

        public override void Initialize()
        {
            base.Initialize();
            _isInitialized = true;
        }

        protected virtual void BeforeShow()
        {
            // Tàng hình
            _canvasGroup.alpha = 0f;

            // Khóa click để chống người chơi táy máy trong lúc đang chạy Anim
            _canvasGroup.interactable = false;

            // Khóa tia Raycast thọc xuyên xuống mặt đất dưới bàn cờ
            _canvasGroup.blocksRaycasts = true;
            // Reset màn đen lót dưới nếu có
            if (_darkOverlay != null)
            {
                _darkOverlay.color = new Color(0, 0, 0, 0);
                _darkOverlay.DOFade(0.7f, _animDuration); // 0.7 là độ đen vừa đẹp
            }
        }
        protected virtual void AfterShow()
        {
            // Hoàn tất hiệu ứng, mở khóa cho phép click
            _canvasGroup.interactable = true;
        }
        protected virtual void BeforeHide()
        {
            // Bóp chết click ngay lập tức để người chơi không bấm đúp nút Tắt (Double-click bug)
            _canvasGroup.interactable = false;
            // Xóa màn đen lót dưới
            if (_darkOverlay != null)
            {
                _darkOverlay.DOFade(0f, _animDuration);
            }
        }
        protected virtual void AfterHide()
        {
            // Tắt hẳn Game Object để giải phóng CPU khỏi việc Render UI
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        protected virtual async UniTask PlayInAnim()
        {
            if (_contentPanel != null)
            {
                // Ép nhỏ lại 80% rồi nảy bóp lên 100%
                _contentPanel.localScale = Vector3.one * 0.8f;
                _canvasGroup.DOFade(1f, _animDuration);

                await _contentPanel.DOScale(Vector3.one, _animDuration)
                                   .SetEase(Ease.OutBack)
                                   .AsyncWaitForCompletion();
            }
            else // Phòng hờ trường hợp Sếp lười không gán biến _contentPanel
            {
                await _canvasGroup.DOFade(1f, _animDuration).AsyncWaitForCompletion();
            }
        }
        protected virtual async UniTask PlayOutAnim()
        {
            if (_contentPanel != null)
            {
                // Mờ đi và teo nhỏ lại
                _canvasGroup.DOFade(0f, _animDuration);

                await _contentPanel.DOScale(Vector3.one * 0.8f, _animDuration)
                                   .SetEase(Ease.InBack)
                                   .AsyncWaitForCompletion();
            }
            else
            {
                await _canvasGroup.DOFade(0f, _animDuration).AsyncWaitForCompletion();
            }
        }
    }
}