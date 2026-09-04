using System;
using System.Collections.Generic;

namespace Flavor
{
    public class GatePassHandler : BaseMono
    {
        public event Action OnOpenGate;
        public event Action OnCloseGate;

        private HashSet<IBlockInfo> _passingBlocks = new HashSet<IBlockInfo>();

        public void HandleBlockPass(IBlockController BlockController, IGateController GateController)
        {
            var blockInfo = BlockController.BlockInfo;
            _passingBlocks.Add(BlockController.BlockInfo);
            if (_passingBlocks.Count == 1)
            {
                OnOpenGate?.Invoke();

            }

            BlockController.OnExitedGate(GateController.GateInfo, () =>
            {
                OnBlockFinishedPassing(blockInfo);

            });
        }

        private void OnBlockFinishedPassing(IBlockInfo blockInfo)
        {
            _passingBlocks.Remove(blockInfo);

            if (_passingBlocks.Count == 0)
            {
                OnCloseGate?.Invoke();
            }
        }
    }

}