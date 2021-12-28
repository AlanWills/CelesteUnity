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
            Apply(currentSkin.Value);

            currentSkin.AddOnValueChangedCallback(OnUISkinChanged);
        }

        private void OnDisable()
        {
            currentSkin.RemoveOnValueChangedCallback(OnUISkinChanged);
        }

        #endregion

        private void Apply(UISkin uiSkin)
        {
            text.font = uiSkin.font;
        }

        #region Callbacks

        private void OnUISkinChanged(UISkin newSkin)
        {
            Apply(newSkin);
        }

        #endregion
    }
}