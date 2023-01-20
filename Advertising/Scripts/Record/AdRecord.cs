using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.Advertising
{
    public abstract class AdRecord : ScriptableObject
    {
        #region Properties and Fields

        public string GameId
        {
            get
            {
#if UNITY_ANDROID
                return androidGameId;
#else
                return iOSGameId;
#endif
            }
        }

        public bool AdsEnabled
        {
            get => adsEnabled.Value;
            set => adsEnabled.Value = value;
        }

        public int NumAdPlacements => adPlacements.NumItems;

        [SerializeField] private string androidGameId;
        [SerializeField] private string iOSGameId;
        [SerializeField] private BoolValue adsEnabled;
        [SerializeField] private AdPlacementCatalogue adPlacements;

        #endregion

        public abstract void Initialize(bool testMode);
        public abstract void LoadAdPlacement(AdPlacement adPlacement);
        public abstract void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShow);
        
        public AdPlacement FindAdPlacement(string placementId)
        {
            return adPlacements.FindPlacementById(placementId);
        }

        public AdPlacement GetAdPlacement(int index)
        {
            return adPlacements.GetItem(index);
        }

        protected void LoadAllAdPlacements()
        {
            foreach (AdPlacement adPlacement in adPlacements)
            {
                LoadAdPlacement(adPlacement);
            }
        }
    }
}
