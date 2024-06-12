using Celeste.Advertising.Impls;
using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.Advertising
{
    [CreateAssetMenu(
        fileName = nameof(AdRecord), 
        menuName = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM + "Ad Record",
        order = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM_PRIORITY)]
    public class AdRecord : ScriptableObject
    {
        #region Properties and Fields

        public bool AdsEnabled
        {
            get => adsEnabled.Value;
            set => adsEnabled.Value = value;
        }

        public int NumAdPlacements => adPlacements.NumItems;
        public string GameId => impl.GameId;

        [SerializeField] private BoolValue adsEnabled;
        [SerializeField] private AdPlacementCatalogue adPlacements;

        [Header("Providers")]
        [SerializeField] private EditorAdProvider editorAdProvider;
#if LEGACY_UNITY_ADS
        [SerializeField] private LegacyUnityAdProvider legacyUnityAdProvider;
#endif

        [NonSerialized] private IAdProvider impl = new DisabledAdProvider();
        [NonSerialized] private DisabledAdProvider disabledAdProvider = new DisabledAdProvider();

        #endregion

        public void Initialize()
        {
            adsEnabled.AddValueChangedCallback(OnAdsEnabledValueChanged);

            SelectImpl();
        }

        private void SelectImpl()
        {
            if (adsEnabled.Value)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Assert(editorAdProvider != null, $"{nameof(editorAdProvider)} has not been set on {nameof(AdRecord)}.  Editor errors and Ads not working should be expected.");
                impl = editorAdProvider;
#elif LEGACY_UNITY_ADS
                UnityEngine.Debug.Assert(legacyUnityAdProvider != null, $"{nameof(legacyUnityAdProvider)} has not been set on {nameof(AdRecord)}.  Runtime errors and Ads not working should be expected.");
                impl = legacyUnityAdProvider;
#else
                UnityEngine.Debug.Assert(disabledAdProvider != null, $"{nameof(disabledAdProvider)} has not been set on {nameof(AdRecord)}.  Runtime errors and Ads not working should be expected.");
                impl = disabledAdProvider;
#endif
            }
            else
            {
                impl = disabledAdProvider;
            }

            UnityEngine.Debug.Assert(impl != null, $"No impl is set on {nameof(AdRecord)}.");
            UnityEngine.Debug.Assert(adPlacements != null, $"No {nameof(AdPlacementCatalogue)} is set on {nameof(AdRecord)}.");
            impl.Initialize(adPlacements, LoadAllAdPlacements, null);
        }

        public void LoadAdPlacement(AdPlacement adPlacement)
        {
            impl.LoadAdPlacement(adPlacement);
        }

        protected void LoadAllAdPlacements()
        {
            foreach (AdPlacement adPlacement in adPlacements)
            {
                LoadAdPlacement(adPlacement);
            }
        }

        public void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShow)
        {
            impl.PlayAdPlacement(adPlacement, onShow);
        }
        
        public AdPlacement GetAdPlacement(int index)
        {
            return adPlacements.GetItem(index);
        }

        #region Callbacks

        private void OnAdsEnabledValueChanged(ValueChangedArgs<bool> args)
        {
            SelectImpl();
        }

        #endregion
    }
}
