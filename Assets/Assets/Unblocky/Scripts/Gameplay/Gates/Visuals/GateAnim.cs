using UnityEngine;

namespace Flavor
{
    public class GateAnim : BaseMono
    {
        private Animator _anim;

        protected override void Awake()
        {
            if (_anim == null)
                _anim = GetComponent<Animator>();
        }

        public void PlayAnim(int animHash)
        {
            _anim.SetBool(animHash, true);
        }

        public void StopAnim(int animHash)
        {
            _anim.SetBool(animHash, false);
        }
    }
}