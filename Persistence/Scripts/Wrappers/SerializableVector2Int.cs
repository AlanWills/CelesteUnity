using System;
using UnityEngine;

namespace Celeste.Persistence.Wrappers
{
    [Serializable]
    public struct SerializableVector2Int
    {
        public Vector2Int v => new(x, y);
        public int x;
        public int y;

        public SerializableVector2Int(Vector2Int vector2Int)
        {
            x = vector2Int.x;
            y = vector2Int.y;
        }
    }

    public static class SerializableVector2IntExtensions
    {
        public static SerializableVector2Int ToSerializable(this Vector2Int vector2Int)
        {
            return new SerializableVector2Int(vector2Int);
        }
    }
}