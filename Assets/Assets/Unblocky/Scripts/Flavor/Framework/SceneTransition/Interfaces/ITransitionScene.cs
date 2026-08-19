using System.Collections;

namespace Flavor
{
    public interface ITransitionScene
    {
        IEnumerator TransitionIn();
        void SetProgress(float progress);
        IEnumerator TransitionOut();
    }
}