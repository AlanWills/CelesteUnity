using Celeste.Advertising.UI;
using Celeste.UI.Events;
using System;
using UnityEngine;

namespace Celeste.Advertising.Impls
{
    [CreateAssetMenu(
        fileName = nameof(EditorAdProvider), 
        menuName = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM + "Providers/Editor Ad Provider",
        order = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM_PRIORITY)]
    public class EditorAdProvider : ScriptableObject, IAdProvider
    {
        #region Properties and Fields

        public string GameId => string.Empty;

        [SerializeField] private ShowOverlayEvent showEditorAdOverlay;

        #endregion

        public void Initialize(AdPlacementCatalogue adPlacements, Action onInitializeSuccess, Action<string> onInitializeFail)
        {
            onInitializeSuccess?.Invoke();
        }

        public void LoadAdPlacement(AdPlacement adPlacement)
        {
            if (adPlacement.IsEnabled)
            {
                adPlacement.IsLoaded = true;
            }
        }

        public void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShow)
        {
            if (adPlacement.IsLoaded)
            {
                showEditorAdOverlay.Invoke(new EditorAdOverlayArgs()
                {
                    onShow = onShow
                });
            }
            else
            {
                onShow.Invoke(AdWatchResult.Failed_NotInitialized);
            }
        }
    }
}
