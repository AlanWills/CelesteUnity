using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.Tools
{
    public static class AddressablesUtility
    {
        public static void SetAddressableInfo(this Object o, string group, string address)
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long localID);
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, aaSettings.FindGroup(group));
            entry.address = address;
        }

        public static void SetAddressableInfo(this Object o, string group)
        {
            SetAddressableInfo(o, group, o.name);
        }

        public static void SetAddressableInfo(string path, string group)
        {
            Object o = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (o != null)
            {
                o.SetAddressableInfo(group);
            }
        }

        public static void SetAddressableInfo(string path, string group, string address)
        {
            Object o = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (o != null)
            {
                o.SetAddressableInfo(group, address);
            }
        }

        public static void SetAddressableLabel(this Object o, string group, string label, bool enabled)
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, aaSettings.FindGroup(group));
            entry.SetLabel(label, enabled);
        }

        public static bool AddressableResourceExists<T>(string key)
        {
            foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.address == key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
