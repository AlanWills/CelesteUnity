using Celeste.Advertising.Persistence;
using Celeste.Events;
using Celeste.Parameters;
using Celeste.Persistence;
using Celeste.RemoteConfig;
using UnityEngine;

namespace Celeste.Advertising
{
    public class AdsManager : PersistentSceneManager<AdsManager, AdsManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "Ads.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private BoolValue adTestMode;
        [SerializeField] private AdRecord adRecord;
        [SerializeField] private RemoteConfigRecord remoteConfigRecord;

        private const string ADS_CONFIG_KEY = "AdsConfig";

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            Load();

            if (!isDebugBuild.Value)
            {
                // Only enter test mode if we have enabled the flag and we are in a debug build
                // We should never be able to allow test mode on release builds, even if the flag is on for some reason
                adTestMode.Value = false;
            }

            SyncAdsEnabledFromRemoteConfig();

            adRecord.Initialize();
        }

        protected override void OnDestroy()
        {
            adRecord.Shutdown();

            base.OnDestroy();
        }

        #endregion

        #region Save/Load

        protected override AdsManagerDTO Serialize()
        {
            return new AdsManagerDTO(adTestMode.Value);
        }

        protected override void Deserialize(AdsManagerDTO dto)
        {
            adTestMode.Value = dto.adsTestMode;
        }

        protected override void SetDefaultValues()
        {
            adTestMode.Value = isDebugBuild.Value;
        }

        #endregion
        
        private void SyncAdsEnabledFromRemoteConfig()
        {
            if (remoteConfigRecord == null)
            {
                return;
            }

            IRemoteConfigDictionary adsConfig = remoteConfigRecord.GetDictionary(ADS_CONFIG_KEY);

            if (adsConfig != null)
            {
                for (int i = 0, n = adRecord.NumAdPlacements; i < n; ++i)
                {
                    AdPlacement adPlacement = adRecord.GetAdPlacement(i);
                    adPlacement.IsEnabled = remoteConfigRecord.GetBool(adPlacement.name, adPlacement.IsEnabled);
                }
            }
        }

        #region Callbacks

        public void OnAdTestModeChanged(ValueChangedArgs<bool> args)
        {
            Save();
        }

        public void OnRemoteConfigDataChanged()
        {
            SyncAdsEnabledFromRemoteConfig();
        }

        #endregion
    }
}
