using UnityEngine;

namespace Flavor
{
    public class BlockDirectionCondition : BaseMono, IBlockFitCondition, IHasBlockDirection
    {
        private DirectionType _direction;

        public DirectionType Direction => _direction;

	public bool IsMatch(IGateInfo gateInfo)
        {
            return Direction == gateInfo.Direction;
        }
    }
}