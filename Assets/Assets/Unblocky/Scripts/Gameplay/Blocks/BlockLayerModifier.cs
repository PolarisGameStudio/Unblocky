using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public class BlockLayerModifier : BaseMono, IBlockStateListener
    {
        [SerializeField] private List<Collider> _colliders;
        private int _oldLayer;
        private void SetGhostLayer()
        {
            if (_colliders == null || _colliders.Count == 0) return;
            // Lưu lại Layer cũ. 
            // Giả định tất cả Collider của khối này đều chung 1 Layer ban đầu, nên chỉ cần lấy thằng số [0] làm chuẩn.
            _oldLayer = _colliders[0].gameObject.layer;

            int ghostLayer = LayerMask.NameToLayer("DraggingBlock");
            // Quét qua toàn bộ Collider và đổi Layer
            foreach (var col in _colliders)
            {
                if (col != null)
                    col.gameObject.layer = ghostLayer;
            }
        }
        private void SetExitBlockLayer()
        {
            if (_colliders == null || _colliders.Count == 0) return;
            int ignoringLayer = LayerMask.NameToLayer("IgnoringCollider");
            foreach (var col in _colliders)
            {
                if (col != null)
                    col.gameObject.layer = ignoringLayer;
            }
        }
        public void RestoreLayer()
        {
            if (_colliders == null || _colliders.Count == 0) return;
            foreach (var col in _colliders)
            {
                if (col != null)
                    col.gameObject.layer = _oldLayer;
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
            SetExitBlockLayer();
        }
    }
}