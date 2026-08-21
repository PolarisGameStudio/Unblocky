namespace Flavor
{
    public interface IBlockFitCondition
    {
        public bool IsMatch(IGateInfo gateInfo);
    }
}