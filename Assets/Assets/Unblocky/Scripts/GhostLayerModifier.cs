using UnityEngine;

namespace Flavor
{
    public class GhostLayerModifier : MonoBehaviour
    {
        [SerializeField] private DraggableObject _draggable;
        private int _oldLayer;
        private void Awake()
        {
            if (_draggable == null)
            {
                _draggable = GetComponent<DraggableObject>();
            }

        }

        private void Start()
        {
            _draggable.OnDragStarted += SetGhostLayer;
            _draggable.OnDragEnded += RestoreLayer;
        }

        private void OnDestroy()
        {
            _draggable.OnDragStarted -= SetGhostLayer;
            _draggable.OnDragEnded -= RestoreLayer;
        }

        private void SetGhostLayer()
        {
            _oldLayer = gameObject.layer;
            gameObject.layer = LayerMask.NameToLayer("DraggingBlock");
        }

        public void RestoreLayer()
        {
            gameObject.layer = _oldLayer;
        }
    }
}