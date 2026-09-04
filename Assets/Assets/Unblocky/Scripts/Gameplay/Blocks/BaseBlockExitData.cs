using System;
using UnityEngine;

namespace Flavor
{
    [Serializable]
    public class BaseBlockExitData
    {
        public Vector3 TargetPosition;
        public IGateInfo GateInfo;

        public Action OnComplete;
    }
}