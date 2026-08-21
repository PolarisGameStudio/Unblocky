using UnityEngine;

namespace Flavor
{
    public class GateAnim : BaseMono
    {
        private Animator _anim;
        public void PlayAnim(int animHash)
        {
            _anim.SetBool(animHash, true);
        }
    }
}