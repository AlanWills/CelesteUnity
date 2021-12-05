using Celeste.Events;
using Celeste.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Celeste.Twine.UI.EditTwineNodePopupController;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/UI/Edit Twine Node Popup Controller")]
    public class EditTwineNodePopupController : MonoBehaviour, IPopupController
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

        private const string TAGS_DELIMITER = ",";

        private TwineNode twineNode;

        #endregion

        #region IPopupController

        public void OnShow(IPopupArgs args)
        {
            EditTwineNodePopupArgs editTwineNodePopupArgs = (EditTwineNodePopupArgs)args;
            twineNode = editTwineNodePopupArgs.twineNode;
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
            string newName = titleInputField.text;
            string newText = textInputField.text;
            string[] newTags = tagsInputField.text.Split(new string[] { TAGS_DELIMITER }, System.StringSplitOptions.RemoveEmptyEntries);
            TwineNodeLink[] newLinks = CreateLinks(newText);

            twineNode.UpdateData(
                newName,
                newText,
                newTags,
                newLinks);

            if (string.CompareOrdinal(twineNode.text, textInputField.text) != 0)
            {
                // Make this return the links from the text, not modify the node
                followLinkUIManager.Hookup(twineNode);
            }
        }

        public void OnClosePressed()
        {
        }

        #endregion

        private TwineNodeLink[] CreateLinks(string text)
        {
            List<TwineNodeLink> createdLinks = new List<TwineNodeLink>();
            int delimiterStart = text.IndexOf("[[");

            while (delimiterStart != -1)
            {
                int delimiterEnd = text.IndexOf("]]", delimiterStart + 2);
                int startIndex = delimiterStart + 2;
                string linkText = text.Substring(startIndex, delimiterEnd - startIndex);
                int linkDelimiter = linkText.IndexOf("->");

                TwineNodeLink twineNodeLink = new TwineNodeLink();
                createdLinks.Add(twineNodeLink);

                if (linkDelimiter >= 0)
                {
                    // Need to split out display and internal text
                    twineNodeLink.name = linkText.Substring(0, linkDelimiter);
                    twineNodeLink.link = linkText.Substring(linkDelimiter + 2);
                }
                else
                {
                    // Just use the linkText for both name and link
                    twineNodeLink.name = linkText;
                    twineNodeLink.link = linkText;
                }

                delimiterStart = text.IndexOf("[[", delimiterEnd + 2);
            }

            return createdLinks.ToArray();
        }
    }
}