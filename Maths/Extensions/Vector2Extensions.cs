using UnityEngine;

namespace Celeste.Maths
{
    public static class Vector2Extensions
    {
        public static Vector2 RightTangent(this Vector2 vector2)
        {
            Vector2 t = new Vector2();
            t.x = vector2.y;
            t.y = -vector2.x;

            return t;
        }

        public static Vector2 RotateDegrees(this Vector2 vector2, float degrees)
        {
            Vector3 vec = Quaternion.AngleAxis(degrees, Vector3.forward) * vector2;
            return new Vector2(vec.x, vec.y);
        }

        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, vector2.y, 0);
        }

        public static Vector3 ToVector3(this Vector2Int vector2Int)
        {
            return new Vector3Int(vector2Int.x, vector2Int.y, 0);
        }

        public static Vector3Int ToVector3Int(this Vector2Int vector2Int)
        {
            return new Vector3Int(vector2Int.x, vector2Int.y, 0);
        }
    }
}
