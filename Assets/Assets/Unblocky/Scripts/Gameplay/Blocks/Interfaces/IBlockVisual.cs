using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Flavor
{
    public interface IBlockVisual
    {
        public GameObject GameObject { get; }
        public UniTask SetupVisual(BlockSetupInfo info);
    }
}