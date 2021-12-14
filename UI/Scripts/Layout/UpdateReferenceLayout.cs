using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.UI.Layout
{
    [AddComponentMenu("Celeste/UI/Layout/Update Reference Layout")]
    public class UpdateReferenceLayout : MonoBehaviour
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

        private void Update()
        {
            referenceLayout.rectTransform = rectTransform;
        }

        #endregion
    }
}