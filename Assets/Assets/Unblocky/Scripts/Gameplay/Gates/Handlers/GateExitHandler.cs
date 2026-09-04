using System;
using UnityEngine;

namespace Flavor
{

    public class GateExitHandler : BaseMono 
    {
        public event Action<IBlockController, IGateController> OnBlockExitSuccess;

        public void TryToExit(IBlockController blockController, IGateController gateController)
        {
            var blockInfo = blockController.BlockInfo;
            bool isHolding = false;

            var gateInfo = gateController.GateInfo;

            if (gateInfo == null || blockInfo == null) return;

            if (blockInfo is IDragState dragState)
                isHolding = dragState.IsDragging;
            if (blockController.BlockInfo.IsExited || !isHolding) return;
            if (!gateInfo.IsSatifiedConditions(blockInfo)) return;
            if (!blockInfo.IsSatifiedConditions(gateInfo)) return;
            OnBlockExitSuccess?.Invoke(blockController, gateController);

        }
    }

}