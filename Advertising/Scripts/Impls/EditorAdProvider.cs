using Celeste.Advertising.UI;
using Celeste.Events;
using System;
using UnityEngine;

namespace Celeste.Advertising.Impls
{
    [CreateAssetMenu(fileName = nameof(EditorAdProvider), menuName = "Celeste/Advertising/Providers/Editor Ad Provider")]
    public class EditorAdProvider : ScriptableObject, IAdProvider
    {
        #region Properties and Fields

        public string GameId => string.Empty;

        [SerializeField] private ShowPopupEvent showEditorAdPopup;

        #endregion

        public void Initialize(AdPlacementCatalogue adPlacements, Action onInitializeSuccess, Action<string> onInitializeFail)
        {
            onInitializeSuccess?.Invoke();
        }

        public void LoadAdPlacement(AdPlacement adPlacement)
        {
            adPlacement.IsLoaded = true;
        }

        public void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShow)
        {
            if (adPlacement.IsLoaded)
            {
                showEditorAdPopup.Invoke(new EditorAdPopupArgs()
                {
                    onShow = onShow
                });
            }
            else
            {
                onShow.Invoke(AdWatchResult.Failed_NotReady);
            }
        }
    }
}
