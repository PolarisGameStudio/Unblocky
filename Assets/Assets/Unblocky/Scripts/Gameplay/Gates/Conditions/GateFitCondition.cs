namespace Flavor
{
    public class GateFitCondition : BaseMono, IGateFitCondition
    {
        private int _maxX;
        private int _maxY;
        private DirectionType _direction;

        public bool IsMatch(IBlockInfo blockInfo)
        {

            var (maxX, maxY) = VectorUtils.GetMax(blockInfo.OccupiedOffsets);

            var gateMaxX = _maxX;
            var gateMaxY = _maxY;
            //this.Log($"MaxX {maxX} MaxY {maxY} - gateMaxX {gateMaxX} gateMaxY {gateMaxY} - {IsInsideGate(gateMaxX, maxX, gateMaxY, maxY)}");

            return IsInsideGate(gateMaxX, maxX, gateMaxY, maxY);

        }

        public bool IsInsideGate(int GateMaxX, int BlockMaxX, int GateMaxY, int BlockMaxY)
        {
            if (DirectionType.Horizontal.HasFlag(_direction))
            {
                return BlockMaxY >= 0 && BlockMaxY <= GateMaxY;
            }

            // Nếu cổng nằm dọc (Trên, Dưới), quãng đường đi bằng chiều DÀI (Y/Z) của cục gạch
            if (DirectionType.Vertical.HasFlag(_direction))
            {
                return BlockMaxX >= 0 && BlockMaxX <= GateMaxX;
            }
            return false;
        }

        public void SetupData(IGateInfo info)
        {
            _maxX = info.MaxX;
            _maxY = info.MaxY;
            _direction = info.Direction;
        }
    }
}