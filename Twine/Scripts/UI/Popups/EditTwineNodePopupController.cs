using Celeste.Events;
using Celeste.UI;
using CelesteEditor.Twine;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/UI/Edit Twine Node Popup Controller")]
    public class EditTwineNodePopupController : MonoBehaviour, IPopupController
    {
        #region Args Class

        public class EditTwineNodePopupArgs : ShowPopupArgs
        {
            public TwineNode twineNode;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private TMP_InputField titleInputField;
        [SerializeField] private TMP_InputField textInputField;

        private TwineNode twineNode;

        #endregion

        #region IPopupController

        public void OnShow(ShowPopupArgs args)
        {
            EditTwineNodePopupArgs editTwineNodeArgs = args as EditTwineNodePopupArgs;
            Debug.Assert(editTwineNodeArgs != null, $"Invalid args inputted into {nameof(EditTwineNodePopupController.OnShow)}.");

            twineNode = editTwineNodeArgs.twineNode;
            titleInputField.text = twineNode.name;
            textInputField.text = twineNode.text;
        }

        public void OnHide()
        {
            twineNode = null;
        }

        public void OnConfirmPressed()
        {
            twineNode.name = titleInputField.text;
            twineNode.text = textInputField.text;
            twineNode.OnChanged.Invoke();
        }

        public void OnClosePressed()
        {
        }

        #endregion
    }
}