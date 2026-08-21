using Flavor;
using UnityEngine;
namespace Flavor
{

    public class BlockColorCondition : BaseMono, IBlockFitCondition
    {
        private BlockBehavior _blockBehavior;
        public bool IsMatch(IGateInfo gateInfo)
        {
            return _blockBehavior.Color == gateInfo.Color;
        }
    }
}
