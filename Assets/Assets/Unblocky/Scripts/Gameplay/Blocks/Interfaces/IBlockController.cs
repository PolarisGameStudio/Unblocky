using Cysharp.Threading.Tasks;
using System;

namespace Flavor
{
    public interface IBlockController
    {
        public IBlockInfo BlockInfo { get; }
        public UniTask SetupData(BlockSetupInfo info);
        public void OnExitedGate(IGateInfo gateInfo, Action onComplete = null);
        public event Action<IBlockController> OnControllerExited;
    }
}