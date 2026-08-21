namespace Flavor
{
    public class GateFitCondition : BaseMono, IGateFitCondition
    {
        private GateBehavior _gateBehavior;

        public bool IsMatch(IBlockInfo blockInfo)
        {
            var (maxX, maxY) = VectorUtils.GetMax(blockInfo.OccupiedOffsets);

            var gateMaxX = _gateBehavior.MaxX;
            var gateMaxY = _gateBehavior.MaxY;

            return IsInsideGate(gateMaxX, maxX, gateMaxY, maxY);

        }

        public bool IsInsideGate(int GateMaxX, int BlockMaxX, int GateMaxY, int BlockMaxY)
        {
            bool isFitX = BlockMaxX >= 0 && BlockMaxX <= GateMaxX;
            bool isFitY = BlockMaxY >= 0 && BlockMaxY <= GateMaxY;

            return isFitX && isFitY; // Phải vừa khít X VÀ vừa khít Y
        }

    }
}