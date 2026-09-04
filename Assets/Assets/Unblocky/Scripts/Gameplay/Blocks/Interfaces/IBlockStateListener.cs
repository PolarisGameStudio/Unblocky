namespace Flavor
{
    public interface IBlockStateListener
    {
        public void OnBeginDrag();
        public void OnEndDrag();
        public void OnExitedGate(BaseBlockExitData exitData);
    }
}