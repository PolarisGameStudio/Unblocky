using DG.Tweening;
using UnityEngine;

namespace Flavor
{
    public class BlockExitBehavior : BaseMono, IBlockExitBehavior
    {
        private PlaceableObject _placeableObject;
        private DraggableObject _draggableObject;
        private BlockBehavior _blockBehavior;
        private ClipToGate _clipToGate;
        private Vector3 v;

        protected override void Awake()
        {
            base.Awake();
            _clipToGate = GetComponent<ClipToGate>();
        }


        public void Execute(IGateInfo gateInfo)
        {
            Vector2Int vGrid = Vector2Int.zero;
            if (ServiceLocator.TryGet<BoardSystem>(out var bm))
            {
                bm.UnregisterGrid(_placeableObject);
                vGrid = bm.TranslateWorldToGrid(transform.position);
            }
            _blockBehavior.OnMarkExited();

            BoardInputController.Instance.ClearSelection();
            PlacementController.Instance.PlaceInstant(_placeableObject, vGrid);

            SetClippingGate(gateInfo);

            var vDirection = GateDirectionExtensions.ToWorldVector(gateInfo.Direction);

            Vector3 target = transform.position + Vector3.right;
            transform.DOMove(target, 1f);


        }

        public void SetClippingGate(IGateInfo gateInfo)
        {
            var vWorldPos = gateInfo.GameObject.transform.position;
            var vWorldNormal = GateDirectionExtensions.ToWorldVector(gateInfo.Direction);

            if (vWorldPos == null || vWorldNormal == null) return;

            _clipToGate.SetMatClipping(vWorldPos, vWorldNormal);
        }
    }
}