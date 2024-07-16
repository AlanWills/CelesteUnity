using Celeste.DataStructures;
using Celeste.Events;
using Celeste.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/UI/Edit Twine Node Popup Controller")]
    public class EditTwineNodePopupController : BasePopupController
    {
        #region Args Class

        public struct EditTwineNodePopupArgs : IPopupArgs
        {
            public TwineNode twineNode;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private TMP_InputField titleInputField;
        [SerializeField] private TMP_InputField tagsInputField;
        [SerializeField] private TMP_InputField textInputField;
        [SerializeField] private FollowLinkUIManager followLinkUIManager;
        [SerializeField] private TwineNodeEvent removeTwineNode;

        private const string TAGS_DELIMITER = ",";

        private TwineNode twineNode;
        private string originalNodeName;
        private string originalNodeText;
        private List<string> originalNodeTags = new List<string>();
        private List<TwineNodeLink> originalNodeLinks = new List<TwineNodeLink>();

        #endregion

        #region IPopupController

        protected override void OnShow(IPopupArgs args)
        {
            EditTwineNodePopupArgs editTwineNodePopupArgs = (EditTwineNodePopupArgs)args;
            twineNode = editTwineNodePopupArgs.twineNode;
            originalNodeName = twineNode.Name;
            originalNodeText = twineNode.Text;
            originalNodeTags.AssignFrom(twineNode.Tags);
            originalNodeLinks.AssignFrom(twineNode.Links);

            titleInputField.text = twineNode.Name;
            tagsInputField.text = twineNode.Tags.Count > 0 ? string.Join(TAGS_DELIMITER, twineNode.Tags) : "";
            textInputField.text = twineNode.Text;
            followLinkUIManager.Hookup(twineNode.Links);
        }

        protected override void OnHide()
        {
            twineNode = null;
            originalNodeName = "";
            originalNodeText = "";
            originalNodeTags.Clear();
            originalNodeLinks.Clear();
        }

        protected override void OnClosePressed()
        {
            // Revert the changes to the node
            twineNode.UpdateData(originalNodeName, originalNodeText, originalNodeTags, originalNodeLinks);
        }

        public void OnRemovePressed()
        {
            removeTwineNode.Invoke(twineNode);
        }

        public void OnEndEditTwineNodeTitleText()
        {
            twineNode.Name = titleInputField.text;
        }

        public void OnEndEditTwineNodeTagsText()
        {
            List<string> newTags = new List<string>();

            foreach (string tag in tagsInputField.text.Split(new string[] { TAGS_DELIMITER }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                newTags.Add(tag.Trim());
            }

            twineNode.Tags = newTags;
        }

        public void OnEndEditTwineNodeText()
        {
            string newText = textInputField.text;
            TwineNodeLink[] newLinks = TwineNodeLink.CreateFromText(newText);

            twineNode.Links = newLinks;
            twineNode.Text = newText;

            followLinkUIManager.Hookup(newLinks);
        }

        #endregion
    }
}