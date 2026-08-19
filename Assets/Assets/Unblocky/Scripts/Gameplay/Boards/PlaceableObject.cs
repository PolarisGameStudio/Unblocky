using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> occupiedOffsets;
    [SerializeField] private Vector2Int currentGrid;

    public IReadOnlyList<Vector2Int> OccupiedOffsets => occupiedOffsets;
    public Vector2Int CurrentGrid => currentGrid;

    public void SetCurrentGrid(Vector2Int grid)
    {
        currentGrid = grid;
    }

    public IEnumerable<Vector2Int> GetOccupiedCells(Vector2Int originGrid)
    {
        foreach (var offset in occupiedOffsets)
        {
            yield return originGrid + offset;
        }
    }

}