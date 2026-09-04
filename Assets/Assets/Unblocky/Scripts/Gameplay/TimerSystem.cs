using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public class TimerSystem : BaseSystem
    {
        public static TimerSystem Instance { get; private set; }

        private List<ITimerObserver> _observers = new List<ITimerObserver>();

        protected override void Awake()
        {
            base.Awake();
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Register(ITimerObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Unregister(ITimerObserver observer)
        {
            _observers.Remove(observer);
        }

        public override void Tick()
        {
            base.Tick();
            float dt = Time.deltaTime;

            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                _observers[i].OnTimerPassed(dt);
            }

        }
    }
}