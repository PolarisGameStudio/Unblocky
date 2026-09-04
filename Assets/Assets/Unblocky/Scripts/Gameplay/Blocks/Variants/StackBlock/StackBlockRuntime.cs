using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    [Serializable]
    public class StackBlockRuntime
    {
        public bool IsExited;
        public List<StackItemRuntime> Items;
    }
}