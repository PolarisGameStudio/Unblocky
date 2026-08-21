using UnityEngine;

namespace Flavor
{
    public class BlockVisual : BaseMono
    {
        private MeshRenderer _mesh;
        private DraggableObject _dragObject;

        private Outline _outline;

        protected override void Awake()
        {
            if (_dragObject == null)
                _dragObject = GetComponent<DraggableObject>();
            if (_outline == null)
                _outline = GetComponent<Outline>();
        }

        public override void Initialize()
        {
            base.Initialize();
            _outline.enabled = false;
        }

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            _dragObject.OnDragStarted += ShowOutline;
            _dragObject.OnDragEnded += HideOutline;
        }

        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            _dragObject.OnDragStarted -= ShowOutline;
            _dragObject.OnDragEnded -= HideOutline;
        }

        private void ShowOutline()
        {
            _outline.enabled = true;
        }

        private void HideOutline()
        {
            _outline.enabled = false;
        }

        // có function set Visual(GameColor _color);
    }

}