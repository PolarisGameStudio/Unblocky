using System;

namespace Flavor
{
    public static class GameplayEvent
    {
        public static event Action OnStartPlaying;

        public static void OnTriggerStartPlaying()
        {
            OnStartPlaying?.Invoke();
        }
    }
}