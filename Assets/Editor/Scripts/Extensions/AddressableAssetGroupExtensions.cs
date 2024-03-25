using CelesteEditor.Tools;
using UnityEditor;
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

        public static void SetBuildPath(this AddressableAssetGroup group, string buildPathId)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            
            if (group.TryGetSchema<BundledAssetGroupSchema>(out var bundledAssets))
            {
                bundledAssets.BuildPath.SetVariableById(addressableSettings, buildPathId);
            }
        }

        public static void SetLoadPath(this AddressableAssetGroup group, string loadPathId)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;

            if (group.TryGetSchema<BundledAssetGroupSchema>(out var bundledAssets))
            {
                bundledAssets.LoadPath.SetVariableById(addressableSettings, loadPathId);
            }
        }
    }
}