using Celeste.Tools;
using Celeste.Utils;
using UnityEngine;

namespace Celeste.UI.Layout
{
    [ExecuteInEditMode]
    [AddComponentMenu("Celeste/UI/Layout/Adjust Rect Using Reference Layout")]
    public class AdjustRectUsingReferenceLayout : MonoBehaviour
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
            if (referenceLayout != null)
            {
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isUpdating && !UnityEditor.EditorApplication.isCompiling)
#endif
                {
                    rectTransform.CopyLayoutFrom(referenceLayout.rectTransform);
                }
            }
        }
    }
}