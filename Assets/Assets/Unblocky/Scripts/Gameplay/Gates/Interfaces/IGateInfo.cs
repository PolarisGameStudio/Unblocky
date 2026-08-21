using UnityEngine;

namespace Flavor
{
    public interface IGateInfo
    {
        GameColor Color { get; }
        DirectionType Direction { get; }
        GameObject GameObject { get; }
    }
}