using Celeste.Persistence;
using Celeste.RemoteConfig;
using System.Collections.Generic;
using System.Linq;
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
            List<int> enabledFeaturesFromSaveAndRemoteConfig = MergeEnabledFeaturesWithRemoteConfig(dto.enabledFeatures);
            featureRecord.Hookup(featureCatalogue, enabledFeaturesFromSaveAndRemoteConfig);
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
            List<int> enabledFeaturesFromRemoteConfig = GetEnabledFeaturesFromRemoteConfig();
            featureRecord.Hookup(featureCatalogue, enabledFeaturesFromRemoteConfig);
        }

        #endregion

        private List<int> GetEnabledFeaturesFromRemoteConfig()
        {
            List<int> enabledFeatures = new List<int>();

            if (remoteConfigRecord != null)
            {
                IRemoteConfigDictionary featuresConfig = remoteConfigRecord.GetDictionary(FEATURES_CONFIG_KEY);

                if (featuresConfig != null)
                {
                    foreach (Feature feature in featureCatalogue.Items)
                    {
                        if (featuresConfig.GetBool(feature.name, false))
                        {
                            enabledFeatures.Add(feature.Guid);
                        }
                    }
                }
            }

            return enabledFeatures;
        }

        private List<int> MergeEnabledFeaturesWithRemoteConfig(IReadOnlyList<int> desiredEnabledFeatures)
        {
            List<int> enabledFeatures = new List<int>();

            if (remoteConfigRecord != null)
            {
                IRemoteConfigDictionary featuresConfig = remoteConfigRecord.GetDictionary(FEATURES_CONFIG_KEY);

                if (featuresConfig != null)
                {
                    foreach (Feature feature in featureCatalogue.Items)
                    {
                        if (featuresConfig.GetBool(feature.name, false) && desiredEnabledFeatures.Contains(feature.Guid))
                        {
                            enabledFeatures.Add(feature.Guid);
                        }
                    }
                }
            }

            return enabledFeatures;
        }

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
