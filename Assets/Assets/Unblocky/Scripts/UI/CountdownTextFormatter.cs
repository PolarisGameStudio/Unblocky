using TMPro;
using UnityEngine;

namespace Flavor
{
    // Yêu cầu bắt buộc phải có trạm phát sóng đứng kế bên
    [RequireComponent(typeof(CountdownUI))]
    public class CountdownTextFormatter : BaseMono
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        private CountdownUI _countdownUI;

        protected override void Awake()
        {
            if (_timerText == null) _timerText = GetComponent<TextMeshProUGUI>();
            if (_countdownUI == null) _countdownUI = GetComponent<CountdownUI>();

            // Tìm thằng Trạm phát sóng và cắm ăng-ten nghe lén
        }

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            if (_countdownUI != null)
                _countdownUI.OnSecondChanged += FormatText;

        }

        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            if (_countdownUI != null)
                _countdownUI.OnSecondChanged -= FormatText;
        }

        // Logic dịch chữ thuần túy
        private void FormatText(float seconds)
        {
            int m = (int)seconds / 60;
            int s = (int)seconds % 60;
            _timerText.text = $"{m:00}:{s:00}";
        }
    }
}
