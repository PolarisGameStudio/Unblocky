using UnityEngine;

namespace Flavor
{
    public class BlockDirectionCondition : BaseMono, IBlockFitCondition, IHasBlockDirection
    {
        private DirectionType _direction;

        public DirectionType Direction => _direction;

        public void Init(BlockFitConditionContext blockFitConditionContext)
        {
            _direction = blockFitConditionContext.Direction;
        }

        public bool IsMatch(IGateInfo gateInfo)
        {
            return Direction.HasFlag(gateInfo.Direction);
        }
    }
}