using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Maths
{
    public static class Intersections
    {
        public static int CircleCircle(Vector2 c0, float r0, Vector2 c1, float r1, out Vector2 intersection1, out Vector2 intersection2)
        {
            // Find the distance between the centers.
            double dx = c0.x - c1.x;
            double dy = c0.y - c1.y;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            if (Math.Abs(dist - (r0 + r1)) < 0.00001)
            {
                intersection1 = Vector2.Lerp(c0, c1, r0 / (r0 + r1));
                intersection2 = intersection1;
                return 1;
            }

            // See how many solutions there are.
            if (dist > r0 + r1)
            {
                // No solutions, the circles are too far apart.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else if (dist < Math.Abs(r0 - r1))
            {
                // No solutions, one circle contains the other.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else if ((dist == 0) && (r0 == r1))
            {
                // No solutions, the circles coincide.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else
            {
                // Find a and h.
                double a = (r0 * r0 -
                            r1 * r1 + dist * dist) / (2 * dist);
                double h = Math.Sqrt(r0 * r0 - a * a);

                // Find P2.
                double cx2 = c0.x + a * (c1.x - c0.x) / dist;
                double cy2 = c0.y + a * (c1.y - c0.y) / dist;

                // Get the points P3.
                intersection1 = new Vector2(
                    (float)(cx2 + h * (c1.y - c0.y) / dist),
                    (float)(cy2 - h * (c1.x - c0.x) / dist));
                intersection2 = new Vector2(
                    (float)(cx2 - h * (c1.y - c0.y) / dist),
                    (float)(cy2 + h * (c1.x - c0.x) / dist));

                return 2;
            }
        }

        public static bool LineLineIntersection(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2, out Vector3 intersection)
        {
            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parrallel
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }
    }
}
