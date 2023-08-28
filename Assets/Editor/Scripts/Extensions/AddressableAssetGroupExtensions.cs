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

        public static void SetBuildPath(this AddressableAssetGroup group, string buildPath)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            
            if (group.TryGetSchema<BundledAssetGroupSchema>(out var bundledAssets))
            {
                var buildPathName = bundledAssets.BuildPath.GetName(addressableSettings);
                addressableSettings.profileSettings.SetValue(addressableSettings.activeProfileId, buildPathName, buildPath);
                bundledAssets.BuildPath.SetVariableByName(addressableSettings, buildPathName);
            }
        }

        public static void SetLoadPath(this AddressableAssetGroup group, string loadPath)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;

            if (group.TryGetSchema<BundledAssetGroupSchema>(out var bundledAssets))
            {
                var loadPathName = bundledAssets.LoadPath.GetName(addressableSettings);
                addressableSettings.profileSettings.SetValue(addressableSettings.activeProfileId, loadPathName, loadPath);
                bundledAssets.BuildPath.SetVariableByName(addressableSettings, loadPathName);
            }
        }
    }
}