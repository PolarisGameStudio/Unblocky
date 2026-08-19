using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public enum SceneNameType
    {
        None,
        Launcher,
        MainMenu,
        SelectLevel,
        Gameplay
    }

    [CreateAssetMenu(fileName = "SceneTransitionConfig", menuName = "Flavor/Configs/Scene Transition Config")]
    public class SceneTransitionConfig : ScriptableObject
    {
        [Header("Default Fallback (Dùng khi không tìm thấy cặp khớp)")]
        [SerializeField] private List<BaseTransitionScene> _defaultTransitions;
        [TableList(ShowIndexLabels = true)]
        [SerializeField] private List<SceneTransitionRule> _rules = new List<SceneTransitionRule>();

        /// <summary>
        /// Tự động tìm Transition dựa trên Scene hiện tại và Scene đích
        /// </summary>
        public BaseTransitionScene GetTransition(SceneNameType to)
        {
            SceneNameType currentScene = SceneExtensions.GetCurrentSceneType();
            return GetTransition(currentScene, to);
        }

        /// <summary>
        /// Tìm Transition với From và To cụ thể
        /// </summary>
        private BaseTransitionScene GetTransition(SceneNameType from, SceneNameType to)
        {
            // 1. Nếu Scene hiện tại không hợp lệ -> lấy Default ngẫu nhiên
            if (from == SceneNameType.None)
            {
                Debug.LogWarning("[SceneTransitionConfig] Scene hiện tại là None, sử dụng Default Transition ngẫu nhiên.");
                return GetRandomTransition(_defaultTransitions);
            }
            // 2. Tìm Rule khớp cặp (From -> To)
            SceneTransitionRule matchedRule = _rules.Find(rule => rule.FromScene == from && rule.ToScene == to);
            // 3. Nếu tìm thấy rule -> lấy ngẫu nhiên 1 cái hợp lệ trong list của Rule
            if (matchedRule != null)
            {
                BaseTransitionScene transition = GetRandomTransition(matchedRule.TransitionPrefab);
                if (transition != null)
                {
                    return transition;
                }
            }
            // 4. Nếu không khớp rule hoặc list của rule rỗng -> Fallback về Default ngẫu nhiên
            Debug.Log($"[SceneTransitionConfig] Không có Rule cho [{from} -> {to}], fallback Default Transition ngẫu nhiên.");
            return GetRandomTransition(_defaultTransitions);
        }


        /// <summary>
        /// Helper: Lọc các phần tử null và lấy ngẫu nhiên 1 Transition trong list
        /// </summary>
        private BaseTransitionScene GetRandomTransition(List<BaseTransitionScene> transitions)
        {
            if (transitions == null || transitions.Count == 0)
                return null;
            // Lọc bỏ null phòng khi trên Inspector tạo element nhưng chưa kéo prefab vào
            List<BaseTransitionScene> validList = transitions.FindAll(t => t != null);
            if (validList.Count == 0)
                return null;
            int randomIndex = UnityEngine.Random.Range(0, validList.Count);
            return validList[randomIndex];
        }


    }
    [Serializable]
    public class SceneTransitionRule
    {
        [TableColumnWidth(80, Resizable = false)]
        public SceneNameType FromScene;
        [TableColumnWidth(80, Resizable = false)]
        public SceneNameType ToScene;
        public List<BaseTransitionScene> TransitionPrefab;
    }
}


