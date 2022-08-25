using Celeste.Persistence;
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

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            featureRecord.Hookup(featureCatalogue);

            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            featureRecord.Shutdown(featureCatalogue);
        }

        #endregion

        #region Save/Load

        protected override void Deserialize(FeatureManagerDTO dto)
        {
            foreach (int guid in dto.enabledFeatures)
            {
                featureRecord.SetFeatureEnabled(guid, true);
            }
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
        }

        #endregion
    }
}
