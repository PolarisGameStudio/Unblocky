using System;

namespace Flavor
{
    [Serializable]
    public class GateConfig
    {
        public GameColor Color;
        public DirectionType Direction;
        public int MaxX;
        public int MaxY;
    }
}