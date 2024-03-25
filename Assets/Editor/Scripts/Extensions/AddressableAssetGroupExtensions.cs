using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace CelesteEditor.Assets
{
    public static class AddressableAssetGroupExtensions
    {
        public static bool TryGetSchema<T>(this AddressableAssetGroup group, out T schema) where T : AddressableAssetGroupSchema
        {
            if (group.HasSchema<T>())
            {
                schema = group.GetSchema<T>();
                return true;
            }

            schema = default;
            return false;
        }

        public static void SetBuildPath(this AddressableAssetGroup group, string buildPathVariableName)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            
            if (group.TryGetSchema<BundledAssetGroupSchema>(out var bundledAssets))
            {
                bool setCorrectly = bundledAssets.BuildPath.SetVariableByName(addressableSettings, buildPathVariableName);
                UnityEngine.Debug.Assert(setCorrectly, $"Could not set Build Path using variable with name '{buildPathVariableName}'.  Check the warnings for more details from Unity Addressables.");
            }
        }

        public static void SetLoadPath(this AddressableAssetGroup group, string loadPathVariableName)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;

            if (group.TryGetSchema<BundledAssetGroupSchema>(out var bundledAssets))
            {
                bool setCorrectly = bundledAssets.LoadPath.SetVariableByName(addressableSettings, loadPathVariableName);
                UnityEngine.Debug.Assert(setCorrectly, $"Could not set Load Path using variable with name '{loadPathVariableName}'.  Check the warnings for more details from Unity Addressables.");
            }
        }
    }
}