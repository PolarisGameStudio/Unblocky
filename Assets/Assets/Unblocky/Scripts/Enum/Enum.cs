using UnityEngine;

public enum GameColor
{
    None, Red, Green, Blue, Yellow, Pink, Purple
}

public enum DirectionType
{
    None, Left, Right, Up, Down



}



#region Direction Extensions
public static class GateDirectionExtensions
{
    public static Vector3 ToWorldVector(this DirectionType direction)
    {
        return direction switch
        {
            DirectionType.Left => Vector3.left,
            DirectionType.Right => Vector3.right,
            DirectionType.Up => Vector3.forward,
            DirectionType.Down => Vector3.back,
            _ => Vector3.zero
        };
    }

    public static Vector2Int ToGridVector(this DirectionType direction)
    {
        return direction switch
        {
            DirectionType.Left => new Vector2Int(-1, 0),
            DirectionType.Right => new Vector2Int(1, 0),
            DirectionType.Up => new Vector2Int(0, 1),
            DirectionType.Down => new Vector2Int(0, -1),
            _ => Vector2Int.zero
        };
    }
}

#endregion
