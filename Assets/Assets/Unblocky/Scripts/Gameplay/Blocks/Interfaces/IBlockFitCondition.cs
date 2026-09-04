namespace Flavor
{
    public interface IBlockFitCondition
    {
        public void Init(BlockFitConditionContext blockFitConditionContext);
        public bool IsMatch(IGateInfo gateInfo);
    }

}