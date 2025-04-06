using Celeste.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace DatingBros.PhoneSimulation.UI
{
    [ExecuteAlways]
    public class MaxSizeContentSizeFitter : ContentSizeFitter
    {
        [SerializeField] private float maxWidth = 100;
        [SerializeField] private float maxHeight = 100;
        [SerializeField] private RectTransform rectTransform;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            this.TryGet(ref rectTransform);
        }
#endif

        public override void SetLayoutHorizontal()
        {
            base.SetLayoutHorizontal();

            if (horizontalFit != FitMode.Unconstrained && rectTransform.sizeDelta.x > maxWidth)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
            }
        }

        public override void SetLayoutVertical()
        {
            base.SetLayoutVertical();
            
            if (verticalFit != FitMode.Unconstrained && rectTransform.sizeDelta.y > maxHeight)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
            }
        }
    }
}