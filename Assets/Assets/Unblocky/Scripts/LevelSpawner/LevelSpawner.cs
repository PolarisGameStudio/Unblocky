using Cysharp.Threading.Tasks;
using Flavor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : BaseMono
{
    [Header("References")]
    [SerializeField] private PlacementController _placementController;
    [SerializeField] private BoardSpawner _boardSpawner;
    private LevelSystem _levelSystem;


    private readonly List<PlaceableObject> _spawnedObjects = new();

    public void SetData(LevelSystem levelSystem)
    {
        _levelSystem = levelSystem;
    }

    [Button]
    public async UniTask SpawnLevel()
    {
        var test = _levelSystem.GetLevelConfig;
        ClearLevel();
        _boardSpawner.SpawnVisualGrid(test.BoardSize.x, test.BoardSize.y);

        foreach (var wallConfig in test.Walls)
        {
            if (wallConfig == null) continue;

            var wallPosSpawn = wallConfig.vPosSpawn;
            var wallName = wallConfig.Name;
            GameObject spawnedObj = await AddressablesExtensions.InstantiateAsync(wallName);

            spawnedObj.transform.position = new Vector3(wallPosSpawn.x, 0, wallPosSpawn.y);
        }

        foreach (var gateConfig in test.Gates)
        {
            this.Log($"[GatesCount] 1");
            if (gateConfig == null) continue;

            var gatePosSpawn = gateConfig.vPosSpawn;
            var gateName = gateConfig.Name;
            GameObject spawnedObj = await AddressablesExtensions.InstantiateAsync(gateName);

            if (spawnedObj.TryGetComponent<IGateController>(out var gateController))
            {
                await gateController.SetupData(gateConfig);
            }

            spawnedObj.transform.position = new Vector3(gatePosSpawn.x, 0, gatePosSpawn.y);
        }

        foreach (var blockConfig in test.Blocks)
        {
            if (blockConfig == null) continue;

            var blockGridSpawn = blockConfig.GridSpawn;
            var blockName = blockConfig.Name;
            GameObject spawnedObj = await AddressablesExtensions.InstantiateAsync(blockName);
            if (spawnedObj.TryGetComponent<IBlockController>(out var blockController))
            {
                await blockController.SetupData(blockConfig);
            }

            SpawnObjectAt(blockGridSpawn, spawnedObj);
        }
    }

    private void SpawnObjectAt(Vector2Int grid, GameObject obj)
    {
        PlaceableObject placeable = obj.GetComponent<PlaceableObject>();

        if (placeable == null)
        {
            Debug.LogWarning($"{obj.name} has no PlaceableObject.");
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