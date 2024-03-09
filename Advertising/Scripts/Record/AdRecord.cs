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
#if UNITY_ADS
        [SerializeField] private UnityAdProvider unityAdProvider;
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
                impl = editorAdProvider;
#elif UNITY_ADS
                impl = unityAdProvider;
#else
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
