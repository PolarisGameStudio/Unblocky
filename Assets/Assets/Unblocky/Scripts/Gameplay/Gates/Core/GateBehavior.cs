using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public class GateBehavior : BaseMono, IGateInfo
    {
        public GateConfig Config;
        private Vector3 _vDirection;
        private List<IGateFitCondition> _conditions;

        public GameColor Color => Config.Color;
        public int MaxX => Config.MaxX;
        public int MaxY => Config.MaxY;
        public DirectionType Direction => Config.Direction;
        public GameObject GameObject => this.gameObject;

        public bool IsSatifiedConditions(IBlockInfo blockInfo)
        {
            foreach (var condition in _conditions)
            {
                if (condition.IsMatch(blockInfo) == false)
                {
                    return false;
                }

            }
            return true;

        }
    }

}