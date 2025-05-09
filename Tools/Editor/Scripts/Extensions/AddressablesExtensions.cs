﻿using UnityEditor;
#if USE_ADDRESSABLES
using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif
using UnityEngine;

namespace CelesteEditor.Tools
{
    public static class AddressablesExtensions
    {
        public const string REMOTE_BUILD_PATH_VARIABLE = "Remote.BuildPath";
        public const string REMOTE_LOAD_PATH_VARIABLE = "Remote.LoadPath";
        public const string LOCAL_BUILD_PATH_VARIABLE = "Local.BuildPath";
        public const string LOCAL_LOAD_PATH_VARIABLE = "Local.LoadPath";

        public static bool IsAssetAddressable(this Object obj)
        {
#if USE_ADDRESSABLES
            return GetAddressableInfo(obj) != null;
#else
            return false;
#endif
        }

        public static void SetAddressableInfo(this Object o, string group, string address)
        {
#if USE_ADDRESSABLES
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

            if (o == null)
            {
                Debug.LogAssertion($"Trying to set addressable info on a null object.");
                return;
            }

            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long localID);
            
            AddressableAssetGroup assetGroup = aaSettings.FindGroup(group);
            if (assetGroup == null)
            {
                Debug.LogAssertion($"Failed to find group {group} when setting addressable info.");
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                Debug.LogAssertion($"Attempting to set an empty addressable address for object '{o.name}'.");
                return;
            }
            
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, assetGroup);
            Debug.Assert(entry != null, $"Failed to create or move entry for object '{o.name}'.");

            if (entry != null)
            {
                entry.address = address;
            }
#endif
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
#if USE_ADDRESSABLES
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
#endif
        }

#if USE_ADDRESSABLES
        public static AddressableAssetEntry GetAddressableInfo(this Object obj)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return null;
            }

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            return settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
        }
#endif

        public static void SetAddressableLabel(this Object o, string group, string label, bool enabled)
        {
#if USE_ADDRESSABLES
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, aaSettings.FindGroup(group));
            entry.SetLabel(label, enabled);
#endif
        }

        public static void EnableOnlyAddressableLabel(this Object o, string label)
        {
#if USE_ADDRESSABLES
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
#endif
        }

        public static void SetAddressableAddress(this Object o, string address)
        {
#if USE_ADDRESSABLES
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

            if (o == null)
            {
                Debug.LogAssertion($"Failed to set addressable address {address} as a null object was inputted.");
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                Debug.LogAssertion($"Failed to set addressable address for object '{o.name}' as an empty address was inputted.");
                return;
            }

            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
            AddressableAssetEntry entry = aaSettings.CreateOrMoveEntry(guid, aaSettings.DefaultGroup);
            entry.SetAddress(address);
#endif
        }

        public static void MakeAddressable(this Object o)
        {
            SetAddressableAddress(o, AssetDatabase.GetAssetPath(o));
        }

        public static void MakeAddressable(this Object o, string groupName)
        {
            MakeAddressable(o);
            SetAddressableGroup(o, groupName);
        }

        public static bool AddressableResourceExists<T>(string key)
        {
#if USE_ADDRESSABLES
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
#endif
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
        
#if USE_ADDRESSABLES
        public static bool HasLocalBuildPath(this BundledAssetGroupSchema bundledAssetGroupSchema, AddressableAssetSettings settings)
        {
            string localBuildPath = GetAddressablesLocalBuildPath();
            string groupBuildPath = bundledAssetGroupSchema.BuildPath.GetValue(settings);
            return string.CompareOrdinal(localBuildPath, groupBuildPath) == 0;
        }
#endif

#if USE_ADDRESSABLES
        public static bool HasRemoteBuildPath(this BundledAssetGroupSchema bundledAssetGroupSchema, AddressableAssetSettings settings)
        {
            string localBuildPath = GetAddressablesRemoteBuildPath();
            string groupBuildPath = bundledAssetGroupSchema.BuildPath.GetValue(settings);
            return string.CompareOrdinal(localBuildPath, groupBuildPath) == 0;
        }
#endif

        private static string EvaluateProfileSettingsString(string variablePath)
        {
#if USE_ADDRESSABLES
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return string.Empty;
            }

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, variablePath);
            return profileSettings.EvaluateString(settings.activeProfileId, propName);
#else
            return string.Empty;
#endif
        }

        private static void SetProfileSettingsString(string variablePath, string variableValue)
        {
#if USE_ADDRESSABLES
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            profileSettings.SetValue(settings.activeProfileId, variablePath, variableValue);
#endif
        }
    }
}
