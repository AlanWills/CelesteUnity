using Celeste.Tools;
using Celeste.Utils;
using System.Collections;
using UnityEngine;

namespace Celeste.UI.Layout
{
    [AddComponentMenu("Celeste/UI/Layout/Adjusted Rect From Reference Layout")]
    public class AdjustedRectFromReferenceLayout : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ReferenceLayout referenceLayout;
        [SerializeField] private RectTransform rectTransform;

        #endregion

        private void OnValidate()
        {
            this.TryGet(ref rectTransform);
        }

        private void Update()
        {
            rectTransform.CopyLayoutFrom(referenceLayout.rectTransform);
        }
    }
}