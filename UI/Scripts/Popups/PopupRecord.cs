using Celeste.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.UI.Popups
{
    [CreateAssetMenu(fileName = nameof(PopupRecord), menuName = "Celeste/UI/Popups/Popup Record")]
    public class PopupRecord : ScriptableObject
    {
        #region Structs

        private struct PopupEntry
        {
            public bool IsValid => popup != null;

            public Popup popup;
            public IPopupArgs popupArgs;

            public PopupEntry(Popup popup, IPopupArgs popupArgs)
            {
                this.popup = popup;
                this.popupArgs = popupArgs;
            }
        }

        #endregion

        #region Properties and Fields

        private PopupEntry currentPopup;
        private Stack<PopupEntry> popupStack = new Stack<PopupEntry>();

        #endregion

        public void OnPopupShown(Popup popup, IPopupArgs popupArgs)
        {
            Push(popup, popupArgs);
        }

        public void OnPopupHidden(Popup popup)
        {
            Debug.Assert(currentPopup.IsValid && currentPopup.popup == popup, $"Trying to hide a popup without one currently active.");
            Pop();
        }

        private void Push(Popup popup, IPopupArgs popupArgs)
        {
            if (currentPopup.IsValid)
            {
                currentPopup.popup.Hide();
                popupStack.Push(currentPopup);
            }

            currentPopup = new PopupEntry(popup, popupArgs);
        }

        private void Pop()
        {
            if (popupStack.TryPop(out PopupEntry result))
            {
                result.popup.Show(result.popupArgs);
                currentPopup = result;
            }
            else
            {
                currentPopup = new PopupEntry();
            }
        }
    }
}
