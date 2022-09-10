using Celeste.Constants;
using Celeste.Events;
using Celeste.Parameters;
using Celeste.Tools;
using Celeste.UI.Parameters;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Skin
{
    [ExecuteInEditMode, DisallowMultipleComponent]
    [AddComponentMenu("Celeste/UI/Skin/UI Skin Text")]
    public class UISkinText : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UISkinValue currentSkin;
        [SerializeField] private ID textType;

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
                currentSkin.AddValueChangedCallback(OnUISkinChanged);
            }
        }

        private void OnDisable()
        {
            if (currentSkin != null)
            {
                currentSkin.RemoveValueChangedCallback(OnUISkinChanged);
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
            uiSkin.ApplyTextSettings(text, textType);
        }

        #region Callbacks

        private void OnUISkinChanged(ValueChangedArgs<UISkin> newSkin)
        {
            Apply(newSkin.newValue);
        }

        #endregion
    }
}