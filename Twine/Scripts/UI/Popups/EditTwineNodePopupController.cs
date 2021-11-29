using Celeste.Events;
using Celeste.UI;
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
        [SerializeField] private TMP_InputField tagsInputField;
        [SerializeField] private TMP_InputField textInputField;
        [SerializeField] private FollowLinkUIManager followLinkUIManager;

        private const string TAGS_DELIMITER = ",";

        private TwineNode twineNode;

        #endregion

        #region IPopupController

        public void OnShow(ShowPopupArgs args)
        {
            EditTwineNodePopupArgs editTwineNodeArgs = args as EditTwineNodePopupArgs;
            UnityEngine.Debug.Assert(editTwineNodeArgs != null, $"Invalid args inputted into {nameof(EditTwineNodePopupController.OnShow)}.");

            twineNode = editTwineNodeArgs.twineNode;
            titleInputField.text = twineNode.name;
            tagsInputField.text = twineNode.tags.Count > 0 ? string.Join(TAGS_DELIMITER, twineNode.tags) : "";
            textInputField.text = twineNode.text;
            followLinkUIManager.Hookup(twineNode);
        }

        public void OnHide()
        {
            twineNode = null;
        }

        public void OnConfirmPressed()
        {
            twineNode.name = titleInputField.text;
            twineNode.tags.Clear();
            twineNode.tags.AddRange(tagsInputField.text.Split(new string[] { TAGS_DELIMITER }, System.StringSplitOptions.RemoveEmptyEntries));
            twineNode.text = textInputField.text;
            twineNode.OnChanged.Invoke();
        }

        public void OnClosePressed()
        {
        }

        #endregion
    }
}