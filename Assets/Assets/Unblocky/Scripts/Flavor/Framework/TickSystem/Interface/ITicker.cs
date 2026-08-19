using System;

namespace Flavor
{
    public interface ITicker
    {
        void EarlyTick();
        void Tick();
        void FixedTick();
        void LateTick();
    }
}