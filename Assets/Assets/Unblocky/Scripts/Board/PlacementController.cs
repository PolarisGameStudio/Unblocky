using Flavor;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public bool PlaceInstant(PlaceableObject placeable, Vector2Int grid)
    {
        if (placeable == null)
            return false;
        ServiceLocator.TryGet<BoardSystem>(out var bm);
        

        if (!bm.CanPlace(placeable, grid))
            return false;

        bm.UnregisterGrid(placeable);

        placeable.SetCurrentGrid(grid);
        bm.RegisterGrid(placeable);

        placeable.transform.position = bm.GridToWorld(grid, 0.8124999f);

        return true;
    }

    public bool PlaceWithDrop(
        PlaceableObject placeable,
        DraggableObject draggable,
        Vector2Int grid)
    {
        if (placeable == null || draggable == null)
            return false;

        ServiceLocator.TryGet<BoardSystem>(out var bm);

        if (!bm.CanPlace(placeable, grid))
            return false;

        bm.UnregisterGrid(placeable);

        placeable.SetCurrentGrid(grid);
        bm.RegisterGrid(placeable);

        Vector3 worldPosition = bm.GridToWorld(grid);
        draggable.MoveToBeforeDrop(worldPosition);

        return true;
    }

}
