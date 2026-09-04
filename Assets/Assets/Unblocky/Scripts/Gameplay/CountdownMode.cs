using System;

namespace Flavor
{
    public class CountdownMode : BaseMono, ITimerObserver
    {
        private float _timeLeft;
        private bool _isRunning = false;

        public event Action OnOutOfTime; // Sự kiện báo hết giờ, để GameplayManager lắng nghe và xử lý
        public event Action<float> OnTimePassed;

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            GameplayManager.Instance.OnPlaying += StartTimer;
            GameplayManager.Instance.OnPause += StopTimer;
            GameplayManager.Instance.OnWin += StopTimer;
        }
        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.OnPlaying -= StartTimer;
                GameplayManager.Instance.OnPause -= StopTimer;
                GameplayManager.Instance.OnWin -= StopTimer;
            }
        }

        public override void DoDisable()
        {
            base.DoDisable();
            StopTimer();
        }

        public void OnTimerPassed(float deltaTime)
        {
            if (!_isRunning) return;
            _timeLeft -= deltaTime;

            OnTimePassed?.Invoke(_timeLeft);

            if (_timeLeft <= 0)
            {
                OnOutOfTime?.Invoke(); // Báo cho GameplayManager biết là hết giờ
                StopTimer();
            }
        }

        public void SetCountdown(float startSecond)
        {
            _timeLeft = startSecond;
        }

        private void StartTimer()
        {
            _isRunning = true;
            TimerSystem.Instance.Register(this); // Đăng ký Hộ khẩu để đếm giờ
        }

        private void StopTimer()
        {
            _isRunning = false;
            TimerSystem.Instance.Unregister(this); // Rút hộ khẩu
        }
    }
}