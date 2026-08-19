using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScenesWindow : EditorWindow
{
    private class SceneItem
    {
        public string name;
        public string path;
    }

    private readonly List<SceneItem> scenes = new();
    private Vector2 scrollPosition;
    private string searchText = "";
    private bool openAdditive = false;

    [MenuItem("Tools/Open Scenes")]
    public static void ShowWindow()
    {
        OpenScenesWindow window = GetWindow<OpenScenesWindow>("Open Scenes");
        window.minSize = new Vector2(260, 300);
        window.RefreshScenes();
    }

    private void OnEnable()
    {
        RefreshScenes();
    }

    private void OnGUI()
    {
        DrawHeader();
        DrawSearchBar();
        DrawSceneList();
    }

    private void DrawHeader()
    {
        EditorGUILayout.Space(6);

        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Open Scenes", EditorStyles.boldLabel);

            if (GUILayout.Button("Refresh", GUILayout.Width(70)))
            {
                RefreshScenes();
            }
        }

        openAdditive = EditorGUILayout.ToggleLeft("Open Additive", openAdditive);

        EditorGUILayout.Space(4);
    }

    private void DrawSearchBar()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Search", GUILayout.Width(50));
            searchText = EditorGUILayout.TextField(searchText);

            if (GUILayout.Button("X", GUILayout.Width(24)))
            {
                searchText = "";
                GUI.FocusControl(null);
            }
        }

        EditorGUILayout.Space(6);
    }

    private void DrawSceneList()
    {
        string currentScenePath = SceneManager.GetActiveScene().path;

        IEnumerable<SceneItem> filteredScenes = scenes;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filteredScenes = filteredScenes.Where(scene =>
                scene.name.ToLower().Contains(searchText.ToLower()));
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        foreach (SceneItem scene in filteredScenes)
        {
            bool isCurrentScene = scene.path == currentScenePath;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 26
            };

            if (isCurrentScene)
            {
                GUI.backgroundColor = new Color(0.45f, 0.85f, 0.45f);
            }

            if (GUILayout.Button(scene.name, buttonStyle))
            {
                OpenScene(scene.path);
            }

            GUI.backgroundColor = Color.white;
        }

        EditorGUILayout.EndScrollView();
    }

    private void RefreshScenes()
    {
        scenes.Clear();

        foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
        {
            if (string.IsNullOrEmpty(buildScene.path))
                continue;

            scenes.Add(new SceneItem
            {
                name = Path.GetFileNameWithoutExtension(buildScene.path),
                path = buildScene.path
            });
        }

        scenes.Sort((a, b) => string.Compare(a.name, b.name, true));

        Repaint();
    }

    private void OpenScene(string scenePath)
    {
        bool canContinue = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        if (!canContinue)
            return;

        OpenSceneMode mode = openAdditive ? OpenSceneMode.Additive : OpenSceneMode.Single;

        EditorSceneManager.OpenScene(scenePath, mode);
    }
}