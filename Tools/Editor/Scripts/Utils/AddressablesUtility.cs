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
        public const string REMOTE_BUILD_PATH_VARIABLE = "Remote.BuildPath";
        public const string REMOTE_LOAD_PATH_VARIABLE = "Remote.LoadPath";
        public const string LOCAL_BUILD_PATH_VARIABLE = "Local.BuildPath";
        public const string LOCAL_LOAD_PATH_VARIABLE = "Local.LoadPath";

        public static bool IsAssetAddressable(this Object obj)
        {
            return GetAddressableInfo(obj) != null;
        }

        public static void SetAddressableInfo(this Object o, string group, string address)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

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
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

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
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return null;
            }

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            return settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
        }

        public static void SetAddressableLabel(this Object o, string group, string label, bool enabled)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, aaSettings.FindGroup(group));
            entry.SetLabel(label, enabled);
        }

        public static void EnableOnlyAddressableLabel(this Object o, string label)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

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
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

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
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return false;
            }

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

        public static string GetAddressablesRemoteBuildPath()
        {
            return EvaluateProfileSettingsString(REMOTE_BUILD_PATH_VARIABLE);
        }

        public static void SetAddressablesRemoteBuildPath(string remoteBuildPath)
        {
            SetProfileSettingsString(REMOTE_BUILD_PATH_VARIABLE, remoteBuildPath);
        }

        public static string GetAddressablesRemoteLoadPath()
        {
            return EvaluateProfileSettingsString(REMOTE_LOAD_PATH_VARIABLE);
        }

        public static void SetAddressablesRemoteLoadPath(string remoteLoadPath)
        {
            SetProfileSettingsString(REMOTE_LOAD_PATH_VARIABLE, remoteLoadPath);
        }

        public static string GetAddressablesLocalBuildPath()
        {
            return EvaluateProfileSettingsString(LOCAL_BUILD_PATH_VARIABLE);
        }

        public static void SetAddressablesLocalBuildPath(string localBuildPath)
        {
            SetProfileSettingsString(LOCAL_BUILD_PATH_VARIABLE, localBuildPath);
        }

        public static string GetAddressablesLocalLoadPath()
        {
            return EvaluateProfileSettingsString(LOCAL_LOAD_PATH_VARIABLE);
        }

        public static void SetAddressablesLocalLoadPath(string localLoadPath)
        {
            SetProfileSettingsString(LOCAL_LOAD_PATH_VARIABLE, localLoadPath);
        }

        private static string EvaluateProfileSettingsString(string variablePath)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return string.Empty;
            }

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, variablePath);
            return profileSettings.EvaluateString(settings.activeProfileId, propName);
        }

        private static void SetProfileSettingsString(string variablePath, string variableValue)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            profileSettings.SetValue(settings.activeProfileId, variablePath, variableValue);
        }
    }
}
