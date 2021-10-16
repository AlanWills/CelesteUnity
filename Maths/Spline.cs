using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Maths
{
	public class Spline
	{
		public const int SEGMENT_COUNT = 16;

		[SerializeField]
		private bool loop;

		public bool Loop
		{
			get
			{
				return loop;
			}
			set
			{
				loop = value;
				if (value == true)
				{
					modes[modes.Length - 1] = modes[0];
					SetControlPoint(0, points[0]);
				}
			}
		}

		public int CurveCount
		{
			get { return (points.Length - 1) / 3; }
			set
            {
				int currentCount = CurveCount;
				if (value == currentCount)
                {
					return;
                }

				if (value > currentCount)
				{
					for (int i = 0; i < value - currentCount; ++i)
					{
						AddCurve();
					}
				}
				else
                {
					for (int i = 0; i < currentCount - value; ++i)
					{
						RemoveLastCurve();
					}
				}
            }
		}

		[SerializeField]
		private Vector3[] points;
		
		[SerializeField]
		private BezierControlPointMode[] modes;

		public int ControlPointCount
		{
			get
			{
				return points.Length;
			}
		}

		public Spline()
        {
			points = new Vector3[4]
			{
				new Vector3(0, 0, 0),
				new Vector3(1, 0, 0),
				new Vector3(2, 0, 0),
				new Vector3(3, 0, 0)
			};

			modes = new BezierControlPointMode[2]
			{
				BezierControlPointMode.Free,
				BezierControlPointMode.Free
			};
        }

		public Vector3 GetControlPoint(int index)
		{
			return points[index];
		}

		public void SetControlPoint(int index, Vector3 point)
		{
			//if (index % 3 == 0)
			//{
			//	Vector3 delta = point - points[index];
			//	if (loop)
			//	{
			//		if (index == 0)
			//		{
			//			points[1] += delta;
			//			points[points.Length - 2] += delta;
			//			points[points.Length - 1] = point;
			//		}
			//		else if (index == points.Length - 1)
			//		{
			//			points[0] = point;
			//			points[1] += delta;
			//			points[index - 1] += delta;
			//		}
			//		else
			//		{
			//			points[index - 1] += delta;
			//			points[index + 1] += delta;
			//		}
			//	}
			//	else
			//	{
   //                 if (index > 0)
   //                 {
   //                     points[index - 1] += delta;
   //                 }
   //                 if (index + 1 < points.Length)
   //                 {
   //                     points[index + 1] += delta;
   //                 }
   //             }
			//}
			points[index] = point;
			EnforceMode(index);
		}

		public BezierControlPointMode GetControlPointMode(int index)
		{
			return modes[(index + 1) / 3];
		}

		public void SetControlPointMode(int index, BezierControlPointMode mode)
		{
			int modeIndex = (index + 1) / 3;
			modes[modeIndex] = mode;

			if (loop)
			{
				if (modeIndex == 0)
				{
					modes[modes.Length - 1] = mode;
				}
				else if (modeIndex == modes.Length - 1)
				{
					modes[0] = mode;
				}
			}
			
			EnforceMode(index);
		}

		private void EnforceMode(int index)
		{
			int modeIndex = (index + 1) / 3;
			BezierControlPointMode mode = modes[modeIndex];
			if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
			{
				return;
			}

			int middleIndex = modeIndex * 3;
			int fixedIndex, enforcedIndex;
			if (index <= middleIndex)
			{
				fixedIndex = middleIndex - 1;
				if (fixedIndex < 0)
				{
					fixedIndex = points.Length - 2;
				}
				enforcedIndex = middleIndex + 1;
				if (enforcedIndex >= points.Length)
				{
					enforcedIndex = 1;
				}
			}
			else
			{
				fixedIndex = middleIndex + 1;
				if (fixedIndex >= points.Length)
				{
					fixedIndex = 1;
				}
				enforcedIndex = middleIndex - 1;
				if (enforcedIndex < 0)
				{
					enforcedIndex = points.Length - 2;
				}
			}

			Vector3 middle = points[middleIndex];
			Vector3 enforcedTangent = middle - points[fixedIndex];
			if (mode == BezierControlPointMode.Aligned)
			{
				enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
			}
			points[enforcedIndex] = middle + enforcedTangent;
		}

		public Vector3 GetPoint(float t)
		{
			int i;
			if (t >= 1f)
			{
				t = 1f;
				i = points.Length - 4;
			}
			else
			{
				t = Mathf.Clamp01(t) * CurveCount;
				i = (int)t;
				t -= i;
				i *= 3;
			}

			//return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
			return Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t);
		}

		public Vector3 GetVelocity(float t)
		{
			int i;
			if (t >= 1f)
			{
				t = 1f;
				i = points.Length - 4;
			}
			else
			{
				t = Mathf.Clamp01(t) * CurveCount;
				i = (int)t;
				t -= i;
				i *= 3;
			}
			//return transform.TransformPoint(Bezier.GetFirstDerivative(
			//	points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
			return Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t);
		}

		public Vector3 GetDirection(float t)
		{
			return GetVelocity(t).normalized;
		}

		public void AddCurve()
		{
			Vector3 point = points[points.Length - 1];
			Array.Resize(ref points, points.Length + 3);
			point.x += 1f;
			points[points.Length - 3] = point;
			point.x += 1f;
			points[points.Length - 2] = point;
			point.x += 1f;
			points[points.Length - 1] = point;

			Array.Resize(ref modes, modes.Length + 1);
			modes[modes.Length - 1] = modes[modes.Length - 2];
			EnforceMode(points.Length - 4);

			if (loop)
			{
				points[points.Length - 1] = points[0];
				modes[modes.Length - 1] = modes[0];
				EnforceMode(0);
			}
		}

		private void RemoveLastCurve()
        {
			Array.Resize(ref points, points.Length - 3);
			Array.Resize(ref modes, modes.Length - 1);
        }

        public void Render(LineRenderer lineRenderer, int positionOffset)
        {
			int linePositionDelta = positionOffset + SEGMENT_COUNT * CurveCount + 1 - lineRenderer.positionCount;
			if (linePositionDelta > 0)
            {
				lineRenderer.positionCount += linePositionDelta;
			}

			for (int i = 0, n = SEGMENT_COUNT * CurveCount; i <= n ; i++)
			{
				float t = i / (float)n;

				// Maybe change this later so the spline is precalculated and we use gradient keys again?  Now we're using splines, straight lines will have segments too
				Vector3 pixel = GetPoint(t);
				lineRenderer.SetPosition(positionOffset + i, pixel);
			}
		}
	}
}
