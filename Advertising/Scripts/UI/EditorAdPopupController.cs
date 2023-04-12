using Celeste.Events;
using Celeste.UI;
using System;
using UnityEngine;

namespace Celeste.Advertising.UI
{
    public struct EditorAdPopupArgs : IPopupArgs
    {
        public Action<AdWatchResult> onShow;
    }

    [AddComponentMenu("Celeste/Advertising/UI/Editor Ad Popup Controller")]
    public class EditorAdPopupController : MonoBehaviour, IPopupController
    {
        private Action<AdWatchResult> onShow;

        public void OnShow(IPopupArgs args)
        {
            onShow = ((EditorAdPopupArgs)args).onShow;
        }

        public void OnHide()
        {
        }

        public void OnConfirmPressed()
        {
            onShow.Invoke(AdWatchResult.Completed);
        }

        public void OnClosePressed()
        {
            onShow.Invoke(AdWatchResult.Skipped);
        }
    }
}
