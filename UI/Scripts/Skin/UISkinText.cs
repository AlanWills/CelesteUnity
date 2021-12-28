using Celeste.Tools;
using Celeste.UI.Parameters;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Skin
{
    [ExecuteInEditMode]
    [AddComponentMenu("Celeste/UI/Skin/UI Skin Text")]
    public class UISkinText : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UISkinValue currentSkin;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref text);
        }

        private void OnEnable()
        {
            Apply();

            if (currentSkin != null)
            {
                currentSkin.AddOnValueChangedCallback(OnUISkinChanged);
            }
        }

        private void OnDisable()
        {
            if (currentSkin != null)
            {
                currentSkin.RemoveOnValueChangedCallback(OnUISkinChanged);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            Apply();
        }
#endif

        #endregion

        public void Apply()
        {
            if (currentSkin != null && currentSkin.Value != null)
            {
                Apply(currentSkin.Value);
            }
        }

        private void Apply(UISkin uiSkin)
        {
            if (text.font != uiSkin.font)
            {
                text.font = uiSkin.font;
            }
        }

        #region Callbacks

        private void OnUISkinChanged(UISkin newSkin)
        {
            Apply(newSkin);
        }

        #endregion
    }
}