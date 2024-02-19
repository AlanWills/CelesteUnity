using Celeste.Events;
using Celeste.Parameters;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Popups
{
    public class StringEntryPopupController : BasePopupController
    {
        #region Properties and Fields

        [SerializeField] private TMP_InputField inputField;
        
        private StringValue nameValue;

        #endregion

        #region IPopupController

        public override void OnShow(IPopupArgs args) 
        {
            StringEntryPopupArgs stringEntryPopupArgs = args as StringEntryPopupArgs;
            Debug.Assert(stringEntryPopupArgs != null, $"No {nameof(StringEntryPopupArgs)} passed to popup {name}.");

            if (stringEntryPopupArgs != null)
            {
                nameValue = stringEntryPopupArgs.stringValue;
                inputField.text = nameValue.Value;
            }
        }

        public override void OnConfirmPressed() 
        {
            if (nameValue != null)
            {
                nameValue.Value = inputField.text;
            }
        }

        #endregion
    }
}
