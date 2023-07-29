using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.Tools
{
    public static class AddressablesUtility
    {
        public static bool IsAssetAddressable(this Object obj)
        {
            return GetAddressableInfo(obj) != null;
        }

        public static void SetAddressableInfo(this Object o, string group, string address)
        {
            Debug.Assert(o != null, $"Trying to set addressable info on a null object.");
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long localID);
            
            AddressableAssetGroup assetGroup = aaSettings.FindGroup(group);
            Debug.Assert(assetGroup != null, $"Failed to find group {group} when setting addressable info.");

            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, assetGroup);
            Debug.Assert(entry != null, $"Failed to create or move entry for object {o.name}.");

            if (entry != null)
            {
                entry.address = address;
            }
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

        public static void SetAddressableGroup(this Object o, string group)
        {
            Debug.Assert(o != null, $"Trying to set addressable info on a null object.");
            AddressableAssetEntry assetEntry = GetAddressableInfo(o);

            if (assetEntry != null)
            {
                AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
                AddressableAssetGroup assetGroup = aaSettings.FindGroup(group);
                Debug.Assert(assetGroup != null, $"Failed to find group {group} when setting addressable group for {o.name}.");
                aaSettings.MoveEntry(assetEntry, assetGroup);
            }
            else
            {
                SetAddressableInfo(o, group);
            }
        }

        public static AddressableAssetEntry GetAddressableInfo(this Object obj)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            return settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
        }

        public static void SetAddressableLabel(this Object o, string group, string label, bool enabled)
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, aaSettings.FindGroup(group));
            entry.SetLabel(label, enabled);
        }

        public static void EnableOnlyAddressableLabel(this Object o, string label)
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetEntry entry = GetAddressableInfo(o);
            List<string> labels = aaSettings.GetLabels();

            for (int i = 0, n = labels.Count; i < n; ++i)
            {
                entry.SetLabel(labels[i], string.CompareOrdinal(labels[i], label) == 0);
            }
        }

        public static void SetAddressableAddress(this Object o, string address)
        {
            if (o == null)
            {
                Debug.LogAssertion($"Failed to set addressable address {address} as a null object was inputted.");
                return;
            }

            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, aaSettings.DefaultGroup);
            entry.SetAddress(address);
        }

        public static void MakeAddressable(this Object o)
        {
            SetAddressableAddress(o, AssetDatabase.GetAssetPath(o));
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

        public static string GetAddressablesRemoteBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, "Remote.BuildPath");

            return profileSettings.EvaluateString(settings.activeProfileId, propName);
        }

        public static string GetAddressablesLocalBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, "Local.BuildPath");
            return profileSettings.EvaluateString(settings.activeProfileId, propName);
        }
    }
}
