using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Features.Debug
{
    [CreateAssetMenu(fileName = nameof(FeatureDebugMenu), menuName = CelesteMenuItemConstants.FEATURES_MENU_ITEM + "Debug/Feature Debug Menu", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
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

                using (new GUILayout.HorizontalScope())
                {
                    feature.IsEnabled = GUILayout.Toggle(feature.IsEnabled, feature.name);

                    if (!feature.IsKilled && GUILayout.Button("Kill", GUILayout.ExpandWidth(false)))
                    {
                        feature.Kill();
                    }

                    if (feature.IsKilled && GUILayout.Button("Revive", GUILayout.ExpandWidth(false)))
                    {
                        feature.Revive();
                    }
                }
            }
        }
    }
}
