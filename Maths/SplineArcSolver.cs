using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Maths
{
    public static class SplineArcSolver
    {
        public static void Solve(this Spline spline, Arc arc, Vector3 target)
        {
            float sign = Mathf.Sign(arc.angle);
            int numCurves = Mathf.CeilToInt(sign * arc.angle / 90);
            spline.CurveCount = numCurves + 1;

            Vector3 startDir = arc.from;
            spline.SetControlPoint(0, arc.centre + startDir * arc.radius);

            for (int i = 0; i < numCurves - 1; ++i)
            {
                startDir = Solve(spline, arc.centre, startDir, sign * 90, arc.radius, i * 3);
            }

            startDir = Solve(spline, arc.centre, startDir, arc.angle - sign * 90 * (numCurves - 1), arc.radius, (numCurves - 1) * 3);

            Vector3 endPosition = arc.centre + startDir * arc.radius;
            Vector3 delta = target - endPosition;

            spline.SetControlPoint(numCurves * 3 + 1, endPosition + delta * 0.33f);
            spline.SetControlPoint(numCurves * 3 + 2, endPosition + delta * 0.66f);
            spline.SetControlPoint(numCurves * 3 + 3, endPosition + delta);
        }

        public static void Solve(this Spline spline, Vector3 start, Vector3 target)
        {
            spline.CurveCount = 1;
            
            Vector3 delta = target - start;

            spline.SetControlPoint(0, start);
            spline.SetControlPoint(1, start + delta * 0.33f);
            spline.SetControlPoint(2, start + delta * 0.66f);
            spline.SetControlPoint(3, target);
        }

        private static Vector3 Solve(this Spline spline, Vector3 centre, Vector3 startDirection, float angle, float radius, int index)
        {
            Vector3 start = centre + startDirection * radius;
            Vector3 to = centre + Quaternion.AngleAxis(angle, Vector3.forward) * startDirection * radius;

            float xc = centre.x;
            float yc = centre.y;
            float ax = start.x - xc;
            float ay = start.y - yc;
            float bx = to.x - xc;
            float by = to.y - yc;
            float q1 = ax * ax + ay * ay;
            float q2 = q1 + ax * bx + ay * by;
            float k2 = (4 / 3) * (Mathf.Sqrt(2 * q1 * q2) - q2) / (ax * by - ay * bx);

            float x2 = xc + ax - k2 * ay;
            float y2 = yc + ay + k2 * ax;
            float x3 = xc + bx + k2 * by;
            float y3 = yc + by - k2 * bx;

            spline.SetControlPoint(index + 1, new Vector3(x2, y2, 0));
            spline.SetControlPoint(index + 2, new Vector3(x3, y3, 0));

            startDirection = Quaternion.AngleAxis(angle, Vector3.forward) * startDirection;
            spline.SetControlPoint(index + 3, centre + startDirection * radius);

            return startDirection;
        }
    }
}
