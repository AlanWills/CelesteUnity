using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Options.UI
{
    [AddComponentMenu("Celeste/Options/UI/Float Option UI Controller")]
    public class FloatOptionUIController : MonoBehaviour
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private FloatOption floatOption;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI floatOptionLabel;
        [SerializeField] private Slider floatOptionSlider;
        [SerializeField] private TextMeshProUGUI floatOptionValue;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            floatOptionLabel.text = floatOption.DisplayName;
            floatOptionSlider.value = floatOption.Value;
            floatOptionValue.text = floatOption.Value.ToString();
            floatOptionSlider.onValueChanged.AddListener(OnFloatValueChanged);
        }

        private void OnDisable()
        {
            floatOptionSlider.onValueChanged.RemoveListener(OnFloatValueChanged);
        }

        #endregion

        #region Callbacks

        public void OnFloatValueChanged(float newValue)
        {
            floatOption.Value = newValue;
            floatOptionValue.text = floatOption.Value.ToString();
        }

        #endregion
    }
}
