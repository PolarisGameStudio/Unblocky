using System.Collections;
using UnityEngine;

namespace Flavor
{
    public class AudioTransitionScene : BaseTransitionScene
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _soundIn;   // Ti?ng khi ?óng màn hình
        [SerializeField] private AudioClip _soundOut;  // Ti?ng khi m? màn hình

        public override IEnumerator TransitionIn()
        {
            if (_audioSource != null && _soundIn != null)
            {
                _audioSource.PlayOneShot(_soundIn);
            }
            yield break;
        }

        public override void SetProgress(float progress) { }

        public override IEnumerator TransitionOut()
        {
            if (_audioSource != null && _soundOut != null)
            {
                _audioSource.PlayOneShot(_soundOut);
            }
            yield break;
        }
    }
}
