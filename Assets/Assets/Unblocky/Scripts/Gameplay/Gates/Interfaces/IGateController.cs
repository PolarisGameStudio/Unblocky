using Cysharp.Threading.Tasks;

namespace Flavor
{
    public interface IGateController
    {
        public IGateInfo GateInfo { get; }
        public UniTask SetupData(GateSetupInfo gateSetupInfo);
    }
}