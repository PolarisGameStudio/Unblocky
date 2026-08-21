using Flavor;
using UnityEngine;

public class BoardInputController : MonoBehaviour
{
    public static BoardInputController Instance { get; private set; }

    [Header("Reference Variables")]
    [SerializeField] private PlacementController _placementController;

    [Header("Selection")]
    [SerializeField] private LayerMask _placeableLayer;
    [SerializeField] private Camera _camera;

    [Header("Drag")]
    [SerializeField] private float _liftHeight = 1.5f;

    [SerializeField] private PlaceableObject _currentPlaceable;
    private DraggableObject _currentDraggable;

    private Vector2Int _previousGrid;
    private Vector3 _grabWorldOffset;

    private void Awake()
    {
        Instance = this;

        if (_camera == null)
            _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelect();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateDragTarget();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Drop();
        }
    }

    private void TrySelect()
    {
        if (_currentPlaceable != null)
            return;

        if (_camera == null)
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        bool hitSomething = Physics.Raycast(
            ray,
            out RaycastHit hit,
            100f,
            _placeableLayer
        );

        if (!hitSomething)
            return;

        PlaceableObject placeable =
            hit.collider.GetComponentInParent<PlaceableObject>();

        if (placeable == null)
            return;

        DraggableObject draggable =
            hit.collider.GetComponentInParent<DraggableObject>();

        if (draggable == null)
            return;

        _currentPlaceable = placeable;
        _currentDraggable = draggable;

        _previousGrid = placeable.CurrentGrid;

        if (TryGetMouseWorldOnDragPlane(out Vector3 planeHitPoint))
        {
            _grabWorldOffset = CalculateGrabWorldOffset(
                placeable.transform.position,
                planeHitPoint // Dùng tọa độ của Plane Toán Học thay vì hit.point vật lý
            );
        }
        else
        {
            _grabWorldOffset = Vector3.zero;
        }

        BoardSystem.instance.UnregisterGrid(placeable);

        draggable.BeginDrag();
    }

    private void UpdateDragTarget()
    {
        if (_currentPlaceable == null || _currentDraggable == null)
            return;

        if (!TryGetMouseWorldOnDragPlane(out Vector3 mouseWorld))
            return;

        Vector3 mouseXZ = new Vector3(
            mouseWorld.x,
            0f,
            mouseWorld.z
        );

        Vector3 target = mouseXZ + _grabWorldOffset;
        target.y = _liftHeight;

        _currentDraggable.SetDragTarget(target);
    }

    private void Drop()
    {
        if (_currentPlaceable == null || _currentDraggable == null)
            return;

        Vector3 currentRootWorldPosition = _currentPlaceable.transform.position;

        Vector2Int rootGrid =
            BoardSystem.instance.TranslateWorldToGrid(currentRootWorldPosition);

        bool placed = _placementController.PlaceWithDrop(
            _currentPlaceable,
            _currentDraggable,
            rootGrid
        );

        if (!placed)
        {
            ReturnToPreviousGrid();
        }

        ClearSelection();
    }

    private void ReturnToPreviousGrid()
    {
        bool returned = _placementController.PlaceWithDrop(
            _currentPlaceable,
            _currentDraggable,
            _previousGrid
        );

        if (!returned)
        {
            Debug.LogError(
                $"Cannot return {_currentPlaceable.name} to previous grid {_previousGrid}."
            );
        }
    }

    public void ClearSelection()
    {
        _currentPlaceable = null;
        _currentDraggable = null;
        _grabWorldOffset = Vector3.zero;
    }

    private Vector3 CalculateGrabWorldOffset(
        Vector3 objectRootPosition,
        Vector3 hitPoint)
    {
        Vector3 rootXZ = new Vector3(
            objectRootPosition.x,
            0f,
            objectRootPosition.z
        );

        Vector3 hitXZ = new Vector3(
            hitPoint.x,
            0f,
            hitPoint.z
        );

        return rootXZ - hitXZ;
    }

    private bool TryGetMouseWorldOnDragPlane(out Vector3 worldPosition)
    {
        worldPosition = default;

        if (_camera == null)
            return false;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Plane dragPlane = new Plane(
            Vector3.up,
            new Vector3(0, _liftHeight, 0)
        );


        if (!dragPlane.Raycast(ray, out float distance))
            return false;

        worldPosition = ray.GetPoint(distance);
        return true;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(
            _camera.ScreenPointToRay(Input.mousePosition).origin,
            _camera.ScreenPointToRay(Input.mousePosition).direction * 100f,
            Color.red
        );
    }


}