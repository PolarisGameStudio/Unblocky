namespace Flavor
{
    interface IGateFitCondition
    {
        public bool IsMatch(IBlockInfo blockInfo);
    }
}