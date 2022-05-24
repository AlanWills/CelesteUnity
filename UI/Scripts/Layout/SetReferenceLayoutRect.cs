using Celeste.Tools;
using UnityEngine;

namespace Celeste.UI.Layout
{
    [ExecuteInEditMode]
    [AddComponentMenu("Celeste/UI/Layout/Set Reference Layout Rect")]
    public class SetReferenceLayoutRect : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ReferenceLayout referenceLayout;
        [SerializeField] private RectTransform rectTransform;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref rectTransform);
        }

        private void Awake()
        {
            SetReferenceLayout();
        }

        private void OnEnable()
        {
            referenceLayout.rectTransform = rectTransform;
        }

        #endregion

        public void SetReferenceLayout()
        {
            referenceLayout.rectTransform = rectTransform;
        }
    }
}