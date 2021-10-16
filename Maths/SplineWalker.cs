using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Maths
{
	public enum SplineWalkerMode
	{
		Once,
		Loop,
		PingPong
	}

	public class SplineWalker : MonoBehaviour
	{
		public Spline spline;
		public float duration;
		public bool lookForward;
		public SplineWalkerMode mode;

		private float progress;
		private bool goingForward = true;

		private void Update()
		{
			if (goingForward)
			{
				progress += Time.deltaTime / duration;
				if (progress > 1f)
				{
					if (mode == SplineWalkerMode.Once)
					{
						progress = 1f;
					}
					else if (mode == SplineWalkerMode.Loop)
					{
						progress -= 1f;
					}
					else
					{
						progress = 2f - progress;
						goingForward = false;
					}
				}
			}
			else
			{
				progress -= Time.deltaTime / duration;
				if (progress < 0f)
				{
					progress = -progress;
					goingForward = true;
				}
			}

			Vector3 position = spline.GetPoint(progress);
			transform.localPosition = position;
			if (lookForward)
			{
				transform.LookAt(position + spline.GetDirection(progress));
			}
		}
	}
}
