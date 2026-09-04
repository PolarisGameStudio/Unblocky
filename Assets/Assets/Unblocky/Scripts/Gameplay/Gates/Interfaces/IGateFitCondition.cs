namespace Flavor
{
    interface IGateFitCondition
    {
        public void SetupData(IGateInfo info);
        public bool IsMatch(IBlockInfo blockInfo);
    }
}