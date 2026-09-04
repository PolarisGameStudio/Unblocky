using Flavor;
using UnityEngine;
namespace Flavor
{

    public class BlockColorCondition : BaseMono, IBlockFitCondition
    {
        private GameColor _color;

        public bool IsMatch(IGateInfo gateInfo)
        {
            return _color == gateInfo?.Color;
        }

        public void Init(BlockFitConditionContext blockFitConditionContext)
        {
            _color = blockFitConditionContext.Color;
        }
    }
}
