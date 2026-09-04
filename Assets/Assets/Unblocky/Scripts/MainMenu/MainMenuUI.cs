namespace Flavor
{
    public class MainMenuUI : BaseMono
    {
        public void ButtonPress_Play()
        {
            SceneTransitionManager.Instance.ChangeScene(SceneNameType.Gameplay,
            waitUntil: () => GameplayBootstrapper.IsLoaded);
        }
    }
}