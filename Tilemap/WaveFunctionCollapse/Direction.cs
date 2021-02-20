using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tilemaps.WaveFunctionCollapse
{
    [Serializable]
    public enum Direction
    {
        LeftOf,
        Above,
        RightOf,
        Below,
        AboveLeftOf,
        AboveRightOf,
        BelowRightOf,
        BelowLeftOf
    }

    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Above:
                    return Direction.Below;

                case Direction.Below:
                    return Direction.Above;

                case Direction.LeftOf:
                    return Direction.RightOf;

                case Direction.RightOf:
                    return Direction.LeftOf;

                case Direction.AboveLeftOf:
                    return Direction.BelowRightOf;

                case Direction.AboveRightOf:
                    return Direction.BelowLeftOf;

                case Direction.BelowLeftOf:
                    return Direction.AboveRightOf;

                case Direction.BelowRightOf:
                    return Direction.AboveLeftOf;

                default:
                    Debug.LogErrorFormat("Unhandled Direction: {0}", direction);
                    return Direction.LeftOf;
            }
        }
    }
}
