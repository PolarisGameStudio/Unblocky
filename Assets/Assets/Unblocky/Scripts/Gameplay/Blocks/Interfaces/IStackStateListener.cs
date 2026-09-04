namespace Flavor
{
    public interface IStackStateListener
    {
        public void OnStackUpdated(int index, GameColor Color);
    }
}