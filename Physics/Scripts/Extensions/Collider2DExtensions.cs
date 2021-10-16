using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Physics
{
    public static class Collider2DExtensions
    {
        private static List<Vector2> pointsCache = new List<Vector2>();

        public static void CreateCircle(this EdgeCollider2D edgeCollider2D, float radius, int numPoints)
        {
            if (radius <= 0)
            {
                numPoints = 2;
            }

            EnsurePointsCacheSize(pointsCache, numPoints);

            float angleDelta = Mathf.PI * 2 / (numPoints - 1);
            for (int i = 0, n = numPoints - 1; i < n; ++i)
            {
                Vector2 point = new Vector2(radius * Mathf.Cos(i * angleDelta), radius * Mathf.Sin(i * angleDelta));
                pointsCache.Add(point);
            }
            pointsCache.Add(pointsCache[0]);

            edgeCollider2D.SetPoints(pointsCache);
            pointsCache.Clear();
        }

        private static void EnsurePointsCacheSize(List<Vector2> points, int size)
        {
            pointsCache.Clear();

            if (points.Capacity < size)
            {
                points.Capacity = size;
            }
        }
    }
}
