using System;
using UnityEngine;

namespace Flavor
{

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

    public enum BlockType
    {
        None,
        Normal,
        Arrow,
        Stacked
    }

    public enum GameplayType
    {
        None,
        Setup,
        Playing,
        Pause,
        Win,
        Lose,
    }
}
