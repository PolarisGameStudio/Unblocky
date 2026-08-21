using System;
using UnityEngine;

public enum GameColor
{
    None,
    Red,
    Blue,
    Pink,
    Yellow,
}

[Flags]
public enum DirectionType
{
    None = 0,
    Left = 1 << 0,
    Right = 1 << 1,
    Bot = 1 << 2,
    Top = 1 << 3,

    // Khai báo s?n nhóm n?u c?n:
    Horizontal = Left | Right,
    Vertical = Top | Bot
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
            DirectionType.Top => Vector3.forward,
            DirectionType.Bot => Vector3.back,
            _ => Vector3.zero
        };
    }

    public static Vector2Int ToGridVector(this DirectionType direction)
    {
        return direction switch
        {
            DirectionType.Left => new Vector2Int(-1, 0),
            DirectionType.Right => new Vector2Int(1, 0),
            DirectionType.Top => new Vector2Int(0, 1),
            DirectionType.Bot => new Vector2Int(0, -1),
            _ => Vector2Int.zero
        };
    }
}

#endregion
