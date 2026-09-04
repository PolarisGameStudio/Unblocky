using UnityEngine;

namespace Flavor
{
    public static class AnimHash
    {
        public static int OPENHASH => Animator.StringToHash("Open");
        public static int CLOSEHASH => Animator.StringToHash("Close");
    }
}