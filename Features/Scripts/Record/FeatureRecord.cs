using Celeste.DataStructures;
using Celeste.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(FeatureRecord), menuName = CelesteMenuItemConstants.FEATURES_MENU_ITEM + "Feature Record", order = CelesteMenuItemConstants.FEATURES_MENU_ITEM_PRIORITY)]
    public class FeatureRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumFeatures => features.Count;

        [SerializeField] private Events.Event save;

        [NonSerialized] private List<Feature> features = new List<Feature>();

        #endregion

        public void Hookup(FeatureCatalogue featureCatalogue)
        {
            Hookup(featureCatalogue, Array.Empty<int>());
        }

        public void Hookup(FeatureCatalogue featureCatalogue, IReadOnlyList<int> enabledFeatures)
        {
            for (int i = 0, n = featureCatalogue.NumItems; i < n; ++i)
            {
                Feature feature = featureCatalogue.GetItem(i);
                feature.IsEnabled = enabledFeatures.Contains(i);
                feature.AddOnEnabledChangedCallback(OnEnabledChanged);
                feature.Hookup();
                features.Add(feature);
            }
        }

        public void Shutdown(FeatureCatalogue featureCatalogue)
        {
            for (int i = 0, n = featureCatalogue.NumItems; i < n; ++i)
            {
                Feature feature = featureCatalogue.GetItem(i);
                feature.RemoveOnEnabledChangedCallback(OnEnabledChanged);
                feature.Shutdown();
            }
        }

        public bool IsFeatureEnabled(int featureGuid)
        {
            Feature feature = MustFindFeature(featureGuid);
            return feature != null ? feature.IsEnabled : false;
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
