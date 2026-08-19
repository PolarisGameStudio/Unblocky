using NUnit.Framework;
using System.Collections.Generic;

namespace Flavor
{
    public class TickService : ITicker
    {
        private readonly List<ITickable> _earlyTicks = new List<ITickable>();
        private readonly List<ITickable> _ticks = new List<ITickable>();
        private readonly List<ITickable> _fixedTicks = new List<ITickable>();
        private readonly List<ITickable> _lateTicks = new List<ITickable>();

        public void Register(ITickable tickable, TickMode mode)
        {
            if (mode.HasFlag(TickMode.EarlyTick))
                _earlyTicks.Add(tickable);
            if (mode.HasFlag(TickMode.Tick))
                _ticks.Add(tickable);
            if (mode.HasFlag(TickMode.FixedTick))
                _fixedTicks.Add(tickable);
            if (mode.HasFlag(TickMode.LateTick))
                _lateTicks.Add(tickable);
        }

        public void Unregister(ITickable tickable)
        {
            _earlyTicks.Remove(tickable);
            _ticks.Remove(tickable);
            _fixedTicks.Remove(tickable);
            _lateTicks.Remove(tickable);
        }

        public void Tick()
        {
            for (int i = _ticks.Count - 1; i >= 0; i--)
            {
                _ticks[i].Tick();
            }
        }

        public void FixedTick()
        {
            for (int i = _fixedTicks.Count - 1; i >= 0; i--)
            {
                _fixedTicks[i].FixedTick();
            }
        }

        public void LateTick()
        {
            for (int i = _lateTicks.Count - 1; i >= 0; i--)
            {
                _lateTicks[i].LateTick();
            }
        }

        public void EarlyTick()
        {
            for (int i = _earlyTicks.Count - 1; i >= 0; i--)
            {
                _earlyTicks[i].EarlyTick();
            }
        }
    }
}