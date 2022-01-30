using Celeste.Parameters;
using Celeste.Tools;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Input
{
    [AddComponentMenu("Celeste/UI/Input/String Value Input")]
    public class StringValueInput : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private StringValue stringValue;
        [SerializeField] private TMP_InputField inputField;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref inputField);
        }

        private void OnEnable()
        {
            inputField.text = stringValue.Value;
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnDisable()
        {
            inputField.onEndEdit.RemoveListener(OnEndEdit);
        }

        #endregion

        #region Callbacks

        private void OnEndEdit(string newValue)
        {
            stringValue.Value = newValue;
        }

        #endregion
    }
}