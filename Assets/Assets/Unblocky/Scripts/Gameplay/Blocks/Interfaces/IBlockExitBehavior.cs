using System;
using UnityEngine;
using UnityEngine.Events;

namespace Flavor
{
    public interface IBlockExitBehavior
    {
        public void Init(BlockContext ctx);
        public void Execute(IGateInfo gateInfo, Action onComplete = null);
    }
}