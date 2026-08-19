using Cysharp.Threading.Tasks;
using System.Collections;

namespace Flavor
{
    public interface ISystem : ILifeCycle
    {
        UniTask LoadDataAsync();
    }
}