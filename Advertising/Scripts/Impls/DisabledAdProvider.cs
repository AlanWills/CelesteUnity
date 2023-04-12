using System;

namespace Celeste.Advertising.Impls
{
    public class DisabledAdProvider : IAdProvider
    {
        public string GameId => string.Empty;

        public void LoadAdPlacement(AdPlacement adPlacement)
        {
            adPlacement.IsLoaded = false;
        }

        public void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShow)
        {
            onShow?.Invoke(AdWatchResult.Failed_NotInitialized);
        }
    }
}
