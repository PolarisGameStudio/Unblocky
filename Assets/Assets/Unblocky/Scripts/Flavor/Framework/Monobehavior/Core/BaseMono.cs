using UnityEngine;

namespace Flavor
{
    public abstract class BaseMono : MonoBehaviour, ITickable, ILifeCycle
    {
        [SerializeField] private TickMode _tickMode;

        protected virtual void OnEnable()
        {
            ListeningEvents();
            this.GetApplication().RegisterTickable(this, _tickMode);
            DoEnable();
        }

        protected virtual void OnDisable()
        {
            DoDisable();
            UnlisteningEvents();
            this.GetApplication().UnregisterTickable(this);
        }

        protected virtual void Start()
        {
            Initialize();
        }

        public virtual void DoEnable() { }
        public virtual void DoDisable() { Dispose(); }
        public virtual void Initialize() { }
        public virtual void Dispose() { }
        public virtual void ListeningEvents() { }
        public virtual void UnlisteningEvents() { }
        public virtual void EarlyTick() { }
        public virtual void FixedTick() { }
        public virtual void LateTick() { }
        public virtual void Tick() { }

    }
}