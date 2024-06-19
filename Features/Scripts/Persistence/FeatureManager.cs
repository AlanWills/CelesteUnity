using Celeste.Persistence;
using Celeste.RemoteConfig;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Features.Persistence
{
    public class FeatureManager : PersistentSceneManager<FeatureManager, FeatureManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "Features.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private FeatureCatalogue featureCatalogue;
        [SerializeField] private FeatureRecord featureRecord;
        [SerializeField] private RemoteConfigRecord remoteConfigRecord;

        private const string FEATURES_CONFIG_KEY = "FeaturesConfig";

        #endregion

        #region Unity Methods

        protected override void OnDestroy()
        {
            base.OnDestroy();

            featureRecord.Shutdown(featureCatalogue);
        }

        #endregion

        #region Save/Load

        protected override void Deserialize(FeatureManagerDTO dto)
        {
            featureRecord.Hookup(featureCatalogue, dto.enabledFeatures);

            SyncKilledFeaturesFromRemoteConfig();
        }

        protected override FeatureManagerDTO Serialize()
        {
            List<int> enabledFeatures = new List<int>();

            for (int i = 0, n = featureRecord.NumFeatures; i < n; ++i)
            {
                Feature feature = featureRecord.GetFeature(i);

                if (feature.IsEnabled)
                {
                    enabledFeatures.Add(feature.Guid);
                }
            }

            return new FeatureManagerDTO(enabledFeatures);
        }

        protected override void SetDefaultValues()
        {
            featureRecord.Hookup(featureCatalogue);

            SyncKilledFeaturesFromRemoteConfig();
        }

        #endregion

        private void SyncKilledFeaturesFromRemoteConfig()
        {
            if (remoteConfigRecord == null)
            {
                return;
            }

            IRemoteConfigDictionary featuresConfig = remoteConfigRecord.GetDictionary(FEATURES_CONFIG_KEY);

            if (featuresConfig != null)
            {
                for (int i = 0, n = featureRecord.NumFeatures; i < n; ++i)
                {
                    Feature feature = featureRecord.GetFeature(i);
                    
                    if (featuresConfig.GetBool(feature.name, false))
                    {
                        feature.Kill();
                    }
                    else
                    {
                        feature.Revive();
                    }
                }
            }
        }

        #region Callbacks

        public void OnRemoteConfigChanged()
        {
            SyncKilledFeaturesFromRemoteConfig();
        }

        #endregion
    }
}
