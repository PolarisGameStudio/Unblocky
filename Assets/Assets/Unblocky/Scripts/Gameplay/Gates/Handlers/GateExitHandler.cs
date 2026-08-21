using System;
using UnityEngine;

namespace Flavor
{

    public class GateExitHandler : BaseMono
    {
        public event Action<IBlockInfo, IGateInfo> OnBlockExitSuccess;

        public void TryToExit(IBlockInfo blockInfo, IGateInfo gateInfo)
        {
            var gateBehavior = gateInfo.GameObject.GetComponent<GateBehavior>();
            var blockBehavior = blockInfo.GameObject.GetComponent<BlockBehavior>();

            if (gateBehavior == null || blockBehavior == null) return;

            if (!gateBehavior.IsSatifiedConditions(blockInfo)) return;
            if (!blockBehavior.IsSatifiedConditions(gateInfo)) return;

        }
    }

}