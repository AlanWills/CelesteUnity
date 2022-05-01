using System.Collections;
using UnityEngine;

namespace Celeste.Utils
{
    public static class RectTransformExtensions
    {
        public static void CopyLayoutFrom(this RectTransform targetRectTransform, RectTransform sourceRectTransform)
        {
            if (sourceRectTransform != null)
            {
                targetRectTransform.anchorMin = sourceRectTransform.anchorMin;
                targetRectTransform.anchorMax = sourceRectTransform.anchorMax;
                targetRectTransform.pivot = sourceRectTransform.pivot;
                targetRectTransform.anchoredPosition = sourceRectTransform.anchoredPosition;
                targetRectTransform.sizeDelta = sourceRectTransform.sizeDelta;
            }
        }

        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            // Adjust for pivot

            Vector2 sizeDelta = rectTransform.sizeDelta;
            float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            if ((rectTransform.anchorMax - rectTransform.anchorMin).sqrMagnitude >= 0.01f)
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);

                rectTransformWidth = corners[2].x - corners[0].x;
                rectTransformHeight = corners[2].y - corners[0].y;
            }

            var pivotX = rectTransform.pivot.x;
            var pivotY = rectTransform.pivot.y;

            Vector3 position = rectTransform.position;
            return new Rect(position.x - (pivotX * rectTransformWidth), position.y - (pivotY * rectTransformHeight), rectTransformWidth, rectTransformHeight);
        }
    }
}