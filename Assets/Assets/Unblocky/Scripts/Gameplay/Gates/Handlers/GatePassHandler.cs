namespace Flavor
{
    public class GatePassHandler : BaseMono
    {

        private GateExitHandler _exitHandler;
        private GateAnim _gateAnim;

        public override void ListeningEvents()
        {
            base.ListeningEvents();
            _exitHandler.OnBlockExitSuccess += HandleBlockPass;

        }

        public override void UnlisteningEvents()
        {
            base.UnlisteningEvents();
            _exitHandler.OnBlockExitSuccess -= HandleBlockPass;

        }

        public void HandleBlockPass(IBlockInfo BlockInfo, IGateInfo GateInfo)
        {
            var openHash = 1;
            _gateAnim.PlayAnim(openHash);
            if (BlockInfo.GameObject.TryGetComponent<IBlockExitBehavior>(out var blockExit))
            {
                blockExit.Execute(GateInfo);
            }
        }
    }

}