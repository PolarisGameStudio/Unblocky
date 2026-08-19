using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flavor
{
    public class SceneTransitionManager : MonoBehaviour, ILifeCycle
    {
        public static SceneTransitionManager Instance { get; private set; }
        //[SerializeField] private SceneTransitionConfig _sceneTransitionConfig;
        [SerializeField] private bool _isTransitioning;
        [SerializeField] private BaseTransitionScene _currentTransitionScene;

        public bool IsTransitioning => _isTransitioning;
        public bool IsInitialize;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
        }

        public void ChangeScene(SceneNameType sceneNameType, Func<bool> waitUntil = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (_isTransitioning) return;
            //_currentTransitionScene = _sceneTransitionConfig.GetTransition(sceneNameType);
            StartCoroutine(ChangeSceneRoutine(sceneNameType, waitUntil, loadSceneMode));

        }

        private IEnumerator ChangeSceneRoutine(SceneNameType sceneNameType, Func<bool> waitUntil, LoadSceneMode loadSceneMode)
        {
            _isTransitioning = true;
            // 1. Ch? màn hình che l?i xong
            if (_currentTransitionScene != null)
            {
                yield return _currentTransitionScene.GetComponent<CompositeTransitionScene>().TransitionIn();
            }
            // 2. G?I VÀ CH? SceneUtils LOAD XONG HOÀN TOÀN
            yield return SceneUtils.LoadSceneWithProgressRoutine(
                sceneNameType.ToSceneName(),
                onProgress: (progress) =>
                {
                    _currentTransitionScene?.SetProgress(progress);
                },
                onCompleted: null, // Không c?n callback này n?a vì coroutine t? ?i ti?p xu?ng d??i
                waitUntil: waitUntil,
                mode: loadSceneMode
            );
            // 3. Ch? màn hình m? ra xong
            if (_currentTransitionScene != null)
            {
                yield return _currentTransitionScene.TransitionOut();
            }
            // 4. K?t thúc
            _isTransitioning = false;
        }


        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}