using Celeste.UI;
using Celeste.UI.Events;
using System;
using UnityEngine;

namespace Celeste.Advertising.UI
{
    public struct EditorAdOverlayArgs : IOverlayArgs
    {
        public Action<AdWatchResult> onShow;
    }

    [AddComponentMenu("Celeste/Advertising/UI/Editor Ad Overlay Controller")]
    public class EditorAdOverlayController : MonoBehaviour, IOverlayController
    {
        private Action<AdWatchResult> onShow;

        public void OnShow(IOverlayArgs args)
        {
            onShow = ((EditorAdOverlayArgs)args).onShow;
        }

        public void OnHide()
        {
        }

        #region Callbacks

        public void OnWatchAdSuccessButtonPressed()
        {
            onShow.Invoke(AdWatchResult.Completed);
        }

        public void OnWatchAdSkippedButtonPressed()
        {
            onShow.Invoke(AdWatchResult.Skipped);
        }

        #endregion
    }
}
