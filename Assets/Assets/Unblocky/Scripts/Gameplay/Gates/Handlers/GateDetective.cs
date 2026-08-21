using Unity.VisualScripting;
using UnityEngine;

namespace Flavor
{
    public class GateDetective : BaseMono
    {
        private GateBehavior _gateBehavior;
        private GateExitHandler _gateExitHandler;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<IBlockInfo>(out var blockInfo))
            {
                _gateExitHandler.TryToExit(blockInfo, _gateBehavior);
            }
        }

    }

}