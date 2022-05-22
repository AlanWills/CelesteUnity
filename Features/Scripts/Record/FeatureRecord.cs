using Celeste.DataStructures;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(FeatureRecord), menuName = "Celeste/Features/Feature Record")]
    public class FeatureRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumFeatures => features.Count;

        [SerializeField] private Events.Event save;

        [NonSerialized] private List<Feature> features = new List<Feature>();

        #endregion

        public void Hookup(FeatureCatalogue featureCatalogue)
        {
            for (int i = 0, n = featureCatalogue.NumItems; i < n; ++i)
            {
                Feature feature = featureCatalogue.GetItem(i);
                feature.AddOnEnabledChangedCallback(OnEnabledChanged);
                features.Add(feature);
            }
        }

        public void Shutdown(FeatureCatalogue featureCatalogue)
        {
            for (int i = 0, n = featureCatalogue.NumItems; i < n; ++i)
            {
                Feature feature = featureCatalogue.GetItem(i);
                feature.RemoveOnEnabledChangedCallback(OnEnabledChanged);
            }
        }

        public void SetFeatureEnabled(int featureGuid, bool enabled)
        {
            Feature feature = MustFindFeature(featureGuid);
            if (feature != null)
            {
                feature.IsEnabled = enabled;
            }
        }

        public Feature GetFeature(int index)
        {
            return features.Get(index);
        }

        public Feature FindFeature(int guid)
        {
            return features.Find(x => x.Guid == guid);
        }

        public Feature MustFindFeature(int guid)
        {
            Feature feature = features.Find(x => x.Guid == guid);
            UnityEngine.Debug.Assert(feature != null, $"Could not find feature with guid {guid}.");
            return feature;
        }

        #region Callbacks

        private void OnEnabledChanged(ValueChangedArgs<bool> args)
        {
            save.Invoke();
        }

        #endregion
    }
}
