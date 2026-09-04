using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.STP;

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
        public Vector3 vPos => this.transform.position;


        protected override void Awake()
        {
            base.Awake();

            _conditions = GetComponentsInChildren<IGateFitCondition>().ToList();
        }

        public UniTask SetupData(GateSetupInfo info)
        {
            Config.Color = info.RequiredColor;
            Config.Direction = info.ExitDirection;

            foreach (var condition in _conditions)
            {
                condition.SetupData(this);
            }

            return UniTask.CompletedTask;


        }

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