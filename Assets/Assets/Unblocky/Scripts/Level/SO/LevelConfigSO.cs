using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Flavor/Level Config")]
    public class LevelConfigSO : ScriptableObject
    {
        [Header("Level")]
        public int levelIndex;
        public string levelTitle;

        [Header("Gameplay Setup Info")]
        public GameplaySetupInfo GameplaySetupInfo; // Thông tin thiết lập gameplay

        [Header("Thông số cơ bản")]
        public int LevelID;
        public Vector2Int BoardSize; // Kích thước lưới (ví dụ: 6x6)

        [Header("Chướng ngại vật")]
        public List<WallSetupInfo> Walls; // Tọa độ các bức tường rào
        public List<GateSetupInfo> Gates; // Danh sách các cửa thoát hiểm

        [Header("Các cục gạch")]
        public List<BlockSetupInfo> Blocks; // Danh sách các cục gạch lúc bắt đầu chơi
    }

    [Serializable]
    public class WallSetupInfo
    {
        public string Name;
        public Vector2Int vPosSpawn;
    }

    [Serializable] // Bắt buộc phải có [Serializable] thì Unity mới hiện ra ở Inspector
    public class BlockSetupInfo
    {
        public string Name;
        public BlockType Type;
        public Vector2Int GridSpawn;

        public GameColor Color;

        public DirectionType Direction;

        public List<StackItemSaveData> StackItems;   // Nằm ở ô tọa độ nào lúc bắt đầu

        // Nếu cục gạch này chiếm nhiều hơn 1 ô (Ví dụ: 1x2, 2x2), bạn có thể thêm:
        // public Vector2Int Size = Vector2Int.one; 
    }
    [Serializable]
    public class GateSetupInfo
    {
        public string Name;
        public Vector2Int vPosSpawn;          // Vị trí cửa
        public GameColor RequiredColor;  // Màu yêu cầu để qua cửa (Nếu game bạn cần)
        public DirectionType ExitDirection; // Hướng mở cửa (Nếu cửa có hướng)
    }
}