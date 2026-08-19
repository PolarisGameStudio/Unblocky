using Flavor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public sealed class BoardSystem : BaseSystem
{
    public static BoardSystem instance;

    [Header("Board Settings")]
    [SerializeField] public int _width;
    [SerializeField] public int _height;


    [Header("Runtime")]
    [SerializeField] private bool _spawnGridOnStart = true;

    [ShowInInspector] private readonly Dictionary<Vector2Int, PlaceableObject> _occupiedGrids = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    public void UnregisterGrid(PlaceableObject placeable)
    {
        if (placeable == null)
            return;

        List<Vector2Int> cellsToRemove = new();

        foreach (var pair in _occupiedGrids)
        {
            if (pair.Value == placeable)
                cellsToRemove.Add(pair.Key);
        }

        for (int i = 0; i < cellsToRemove.Count; i++)
        {
            _occupiedGrids.Remove(cellsToRemove[i]);
        }
    }

    public void RegisterGrid(PlaceableObject placeable)
    {
        foreach (Vector2Int cell in GetWorldCells(placeable, placeable.CurrentGrid))
        {
            _occupiedGrids[cell] = placeable;
        }
    }

    public bool CanPlace(PlaceableObject placeable, Vector2Int rootGrid)
    {
        if (placeable == null)
            return false;

        foreach (Vector2Int cell in GetWorldCells(placeable, rootGrid))
        {
            if (!IsInside(cell))
                return false;

            if (_occupiedGrids.TryGetValue(cell, out PlaceableObject owner))
            {
                if (owner != placeable)
                    return false;
            }
        }

        return true;
    }

    private IEnumerable<Vector2Int> GetWorldCells(
        PlaceableObject placeable,
        Vector2Int rootGrid)
    {
        IReadOnlyList<Vector2Int> offsets = placeable.OccupiedOffsets;

        for (int i = 0; i < offsets.Count; i++)
        {
            yield return rootGrid + offsets[i];
        }
    }

    public bool IsInside(Vector2Int grid)
    {
        return grid.x >= 0 &&
               grid.x < _width &&
               grid.y >= 0 &&
               grid.y < _height;
    }

    public Vector3 GridToWorld(Vector2Int grid, float yOffset = 0f)
    {
        Transform origin = transform;

        return origin.position + new Vector3(
            grid.x,
            yOffset,
            grid.y
        );
    }

    public Vector2Int TranslateWorldToGrid(Vector3 worldPosition)
    {
        Transform origin = transform;

        Vector3 local = worldPosition - origin.position;

        int x = Mathf.RoundToInt(local.x);
        int y = Mathf.RoundToInt(local.z);

        return new Vector2Int(x, y);
    }

    public bool IsOccupied(Vector2Int grid)
    {
        return _occupiedGrids.ContainsKey(grid);
    }

    public PlaceableObject GetOwner(Vector2Int grid)
    {
        _occupiedGrids.TryGetValue(grid, out PlaceableObject owner);
        return owner;
    }
}