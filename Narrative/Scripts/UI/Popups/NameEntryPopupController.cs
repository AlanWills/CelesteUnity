using Celeste.Events;
using Celeste.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    public class NameEntryPopupController : MonoBehaviour, IPopupController
    {
        #region Properties and Fields

        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button confirmButton;

        private NameEntryPopupArgs popupArgs;

        #endregion

        #region IPopupController

        public void OnShow(IPopupArgs args)
        {
            popupArgs = args as NameEntryPopupArgs;
            UnityEngine.Debug.Assert(popupArgs != null, $"No {nameof(NameEntryPopupArgs)} passed to popup {name}.");

            if (popupArgs != null)
            {
                inputField.text = popupArgs.nameToEdit.Value;
            }

            ValidateName(inputField.text);
        }

        public void OnHide()
        {
        }

        public void OnClosePressed() { }

        public void OnConfirmPressed()
        {
            if (popupArgs != null)
            {
                popupArgs.nameToEdit.Value = inputField.text;
            }
        }

        #endregion

        private void ValidateName(string name)
        {
            name = name.Trim();

            bool isValid = true;
            string errorString = "";

            if (string.IsNullOrEmpty(name))
            {
                isValid = false;
                errorString = "You must enter a name!";
            }
            else if (popupArgs != null && popupArgs.reservedNames.Exists(x => string.CompareOrdinal(x.Value, name) == 0))
            {
                isValid = false;
                errorString = $"The name '{name}' is not valid!\n\nPlease choose another.";
            }

            errorText.gameObject.SetActive(!isValid);
            errorText.text = errorString;

            confirmButton.interactable = isValid;
            confirmButton.gameObject.SetActive(isValid);
        }

        #region Callbacks

        public void OnEndEditName(string value)
        {
            ValidateName(value);
        }

        #endregion
    }
}