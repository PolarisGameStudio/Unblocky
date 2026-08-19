using System.Collections;
using UnityEngine;

namespace Flavor
{
    public abstract class BaseTransitionScene : MonoBehaviour, ITransitionScene
    {
        public virtual IEnumerator TransitionIn()
        {
            yield break;
        }
        public virtual void SetProgress(float progress)
        {
        }
        public virtual IEnumerator TransitionOut()
        {
            yield break;
        }
    }
}