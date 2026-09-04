using System;
using UnityEngine;

namespace Flavor
{
    public class BlockExitData : BaseBlockExitData
    {
        public int StackIndex;         // Thằng Gạch Thường KHÔNG XÀI, cứ kệ nó!

        public Vector3 TargetPosition { get; set; }
        public IGateInfo GateInfo { get; set; }

        public event Action OnComplete;
    }
}