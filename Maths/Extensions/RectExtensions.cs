using UnityEngine;

namespace Celeste.Maths
{
    public static class RectExtensions
    {
        public static RectInt ToRectInt(this Bounds bounds)
        {
            return new RectInt(
                (int)bounds.min.x,
                (int)bounds.min.y,
                (int)bounds.size.x,
                (int)bounds.size.y);
        }
    }
}
