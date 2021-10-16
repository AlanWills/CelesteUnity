using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.UI
{
	[ExecuteInEditMode]
	[AddComponentMenu("Celeste/UI/Scale Grid")]
	[RequireComponent(typeof(GridLayoutGroup))]
	public class ScaleGrid : MonoBehaviour
	{
		[Range(0, 1)]
		public float widthPercentage;

		[Range(0, 1)]
		public float heightPercentage;

		[SerializeField]
		private GridLayoutGroup gridLayoutGroup;

		float lWidthPercentage = 0;
		float lHeightPercentage = 0;
		Vector2 viewSize = Vector2.zero;

		#region Unity Methods

		private void Start()
		{
			Fix();
		}

		private void Update()
		{
			if (GetMainGameViewSize() != viewSize || widthPercentage != lWidthPercentage || heightPercentage != lHeightPercentage)
			{
				Fix();
			}
		}

        private void OnValidate()
        {
            if (gridLayoutGroup == null)
            {
				gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }
        }

        #endregion

        public void Fix()
		{
			GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
			viewSize = GetMainGameViewSize();

			var valWidth = (int)Mathf.Round(viewSize.x * widthPercentage);
			var valHeight = (int)Mathf.Round(viewSize.y * heightPercentage);

			//Toggle enabled to update screen (is there a better way to do this?)
			grid.cellSize = new Vector2(valWidth, valHeight);
			grid.enabled = false;
			grid.enabled = true;

			lWidthPercentage = widthPercentage;
			lHeightPercentage = heightPercentage;
		}

		public static Vector2 GetMainGameViewSize()
		{
			return new Vector2(Screen.width, Screen.height);
		}
	}
}