using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Options.UI
{
    [AddComponentMenu("Celeste/Options/UI/Bool Option UI Controller")]
    public class BoolOptionUIController : MonoBehaviour
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private BoolOption boolOption;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI boolOptionLabel;
        [SerializeField] private Toggle boolOptionToggle;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            boolOptionLabel.text = boolOption.DisplayName;
            boolOptionToggle.isOn = boolOption.Value;
            boolOptionToggle.onValueChanged.AddListener(OnBoolValueChanged);
        }

        private void OnDisable()
        {
            boolOptionToggle.onValueChanged.RemoveListener(OnBoolValueChanged);
        }

        #endregion

        #region Callbacks

        public void OnBoolValueChanged(bool newValue)
        {
            boolOption.Value = newValue;
        }

        #endregion
    }
}
