using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Flavor
{
    public class GateDetective : BaseMono
    {

        public event Action<IBlockController> OnDetectBlock;

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && rb.TryGetComponent<IBlockController>(out var blockController))
            {
                OnDetectBlock?.Invoke(blockController);
            }
        }

    }

}