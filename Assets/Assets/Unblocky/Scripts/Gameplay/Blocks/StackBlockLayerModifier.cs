using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public class StackBlockLayerModifiers : BaseMono, IBlockStateListener
    {
        [SerializeField] private List<Collider> _colliders;
        private int _oldLayer;
        private void SetGhostLayer()
        {
            if (_colliders == null || _colliders.Count == 0) return;
            _oldLayer = _colliders[0].gameObject.layer;
            int ghostLayer = LayerMask.NameToLayer("DraggingBlock");
            foreach (var col in _colliders)
            {
                if (col != null) col.gameObject.layer = ghostLayer;
            }
        }
        private void SetExitBlockLayer()
        {
            if (_colliders == null || _colliders.Count == 0) return;
            int ignoringLayer = LayerMask.NameToLayer("IgnoringCollider");
            foreach (var col in _colliders)
            {
                if (col != null) col.gameObject.layer = ignoringLayer;
            }
        }
        public void RestoreLayer()
        {
            if (_colliders == null || _colliders.Count == 0) return;
            foreach (var col in _colliders)
            {
                if (col != null) col.gameObject.layer = _oldLayer;
            }
        }
        public void OnBeginDrag()
        {
            SetGhostLayer();
        }
        public void OnEndDrag()
        {
            RestoreLayer();
        }
        public void OnExitedGate(BaseBlockExitData exitData)
        {
            this.Log($"[ExitedGate] Block Layer");

            // Logic Cực Xịn của Sếp: Chỉ xuyên tường khi là cục gạch cuối cùng!
            if (exitData is StackBlockExitData stackExitData)
            {
                if (stackExitData.IsLast)
                {
                    SetExitBlockLayer();
                }
                else
                {
                    RestoreLayer();
                }
            }
        }
    }
}