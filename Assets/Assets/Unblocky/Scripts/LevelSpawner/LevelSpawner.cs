using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class LevelSpawner : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlacementController _placementController;

    [Header("Test Level Data")]
    [OdinSerialize]
    [DictionaryDrawerSettings(
        KeyLabel = "Cell",
        ValueLabel = "Prefab")]
    private Dictionary<Vector2Int, GameObject> levelObjects = new();

    private readonly List<PlaceableObject> _spawnedObjects = new();

    private void Start()
    {
        SpawnLevel();
    }

    [Button]
    public void SpawnLevel()
    {

        ClearLevel();
        //board.SpawnGrid(board.Width, board.Height);

        foreach (var pair in levelObjects)
        {
            Vector2Int grid = pair.Key;
            GameObject prefab = pair.Value;

            if (prefab == null)
                continue;

            SpawnObjectAt(grid, prefab);
        }
    }

    private void SpawnObjectAt(Vector2Int grid, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);

        PlaceableObject placeable = obj.GetComponent<PlaceableObject>();

        if (placeable == null)
        {
            Debug.LogWarning($"{prefab.name} has no PlaceableObject.");
            Destroy(obj);
            return;
        }
        Debug.LogWarning($"[LevelSpawner]-[SpawnObjectAt] grid {grid}");
        _placementController.PlaceInstant(placeable, grid);
        _spawnedObjects.Add(placeable);
    }

    [Button]
    public void ClearLevel()
    {
        for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
        {
            PlaceableObject obj = _spawnedObjects[i];

            if (obj == null)
                continue;

            //board.Remove(obj);

            if (Application.isPlaying)
                Destroy(obj.gameObject);
            else
                DestroyImmediate(obj.gameObject);
        }

        _spawnedObjects.Clear();
    }
}