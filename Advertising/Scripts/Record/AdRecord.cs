using Celeste.Advertising.Impls;
using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.Advertising
{
    [CreateAssetMenu(fileName = nameof(AdRecord), menuName = "Celeste/Advertising/Ad Record")]
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
#if UNITY_EDITOR
        [SerializeField] private EditorAdProvider editorAdProvider;
#endif
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
