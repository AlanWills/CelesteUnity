using System;

namespace Celeste.Advertising.Impls
{
    public interface IAdProvider
    {
        string GameId { get; }

        void LoadAdPlacement(AdPlacement adPlacement);
        void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShow);
    }
}
