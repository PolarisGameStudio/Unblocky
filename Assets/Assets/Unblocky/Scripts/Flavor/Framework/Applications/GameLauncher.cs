using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flavor
{
    public class GameLauncher : MonoBehaviour
    {
        [SerializeField] private MainApplication _mainApplication;
        [SerializeField] private SceneTransitionManager _sceneTransitionManager;

        private void Start()
        {
            _sceneTransitionManager.ChangeScene(SceneNameType.MainMenu,
                waitUntil: () => _mainApplication.IsInitialized);
        }
    }
}