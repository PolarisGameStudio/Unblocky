using System.Collections;

namespace Flavor
{
    public interface ILifeCycle
    {
        // Thay thế cho Awake/Start
        void Initialize();
        // Thay thế cho OnDestroy
        void Dispose();
    }
}