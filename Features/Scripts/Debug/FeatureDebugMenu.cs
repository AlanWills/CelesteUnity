using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Features.Debug
{
    [CreateAssetMenu(fileName = nameof(FeatureDebugMenu), menuName = "Celeste/Features/Debug/Feature Debug Menu")]
    public class FeatureDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private FeatureRecord featureRecord;

        #endregion

        protected override void OnDrawMenu()
        {
            for (int i = 0, n = featureRecord.NumFeatures; i < n; i++)
            {
                Feature feature = featureRecord.GetFeature(i);
                feature.IsEnabled = GUILayout.Toggle(feature.IsEnabled, feature.name);
            }
        }
    }
}
