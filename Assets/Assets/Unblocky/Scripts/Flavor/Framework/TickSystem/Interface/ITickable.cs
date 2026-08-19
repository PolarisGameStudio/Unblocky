using System;

namespace Flavor
{
    public interface ITickable
    {
        public void EarlyTick();
        public void Tick();
        public void FixedTick();
        public void LateTick();
    }

    [Flags]
    public enum TickMode
    {
        None = 0,
        EarlyTick = 1,
        Tick = 2,
        FixedTick = 4,
        LateTick = 8,
    }
}