using DG.Tweening;
using System.Runtime.Serialization;
using UnityEngine;

namespace Flavor
{
    [RequireComponent(typeof(CountdownUI))]
    public class CountdownWarningEffect : BaseMono
    {
        [SerializeField] private int _warningThreshold = 10;

        // Thằng cần bị rung lắc (Có thể là nguyên cái Bảng nền, không nhất thiết là cái Text)
        [SerializeField] private Transform _targetToShake;
        [SerializeField] private CountdownUI _countdownUI;

        protected override void Awake()
        {
            if (_targetToShake == null) _targetToShake = transform;

            // Cắm ăng-ten nghe lén
            if (_countdownUI == null) GetComponent<CountdownUI>().OnSecondChanged += CheckAndPlayEffect;
        }
        public override void ListeningEvents()
        {
            base.ListeningEvents();
            if (_countdownUI != null)
                _countdownUI.OnSecondChanged += CheckAndPlayEffect;

        }

        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            if (_countdownUI != null)
                _countdownUI.OnSecondChanged -= CheckAndPlayEffect;
        }

        private void CheckAndPlayEffect(float seconds)
        {
            // Đạt điều kiện là Múa!
            if (seconds <= _warningThreshold && seconds > 0)
            {
                _targetToShake.DOKill();
                _targetToShake.localScale = Vector3.one;
                _targetToShake.DOPunchScale(Vector3.one * 0.2f, 0.3f);
            }
        }
    }
}
