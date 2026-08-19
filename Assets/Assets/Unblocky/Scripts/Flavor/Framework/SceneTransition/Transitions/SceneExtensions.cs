using Flavor;
using System;
using UnityEngine.SceneManagement;

public static class SceneExtensions
{
    // S? d?ng Switch Expression c?a C# (v?a ng?n v?a t?i ?u)
    public static string ToSceneName(this SceneNameType type)
    {
        return type switch
        {
            SceneNameType.Launcher => "Launcher",
            SceneNameType.MainMenu => "MainMenu",
            SceneNameType.SelectLevel => "SelectLevel",
            SceneNameType.Gameplay => "Gameplay",
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Ch?a c?u hình tên Scene cho: {type}")
        };
    }

    // 2. Từ String -> Enum (Ngược lại)
    public static SceneNameType ToSceneNameType(this string sceneName)
    {
        return sceneName switch
        {
            "Launcher" => SceneNameType.Launcher,
            "MainMenu" => SceneNameType.MainMenu,
            "SelectLevel" => SceneNameType.SelectLevel,
            "Gameplay" => SceneNameType.Gameplay,
            _ => SceneNameType.None
        };
    }

    // 3. Hàm tiện ích lấy ngay Enum của Scene hiện tại
    public static SceneNameType GetCurrentSceneType()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        return currentSceneName.ToSceneNameType();
    }
}