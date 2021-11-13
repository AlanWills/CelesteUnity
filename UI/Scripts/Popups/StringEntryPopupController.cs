using Celeste.Events;
using Celeste.Parameters;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Popups
{
    public class StringEntryPopupController : MonoBehaviour, IPopupController
    {
        #region Properties and Fields

        [SerializeField] private StringValue nameValue;
        [SerializeField] private TMP_InputField inputField;

        #endregion

        #region IPopupController

        public void OnShow(ShowPopupArgs args) 
        {
            inputField.text = nameValue.Value;
        }

        public void OnHide()
        {
        }

        public void OnClosePressed() { }

        public void OnConfirmPressed() 
        {
            nameValue.Value = inputField.text;
        }

        #endregion
    }
}
