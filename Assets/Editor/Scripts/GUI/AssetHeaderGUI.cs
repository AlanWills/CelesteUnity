﻿using Celeste.Assets;
using Celeste.Tools;
using CelesteEditor.Tools;
using System;
using UnityEditor;
#if USE_ADDRESSABLES
using UnityEditor.AddressableAssets;
#endif
using UnityEngine;

namespace CelesteEditor.Assets.GUI
{
    [InitializeOnLoad]
    internal static class AssetHeaderGUI
    {
        private static readonly GUIContent s_AddressableAssetGroupText;

        static AssetHeaderGUI()
        {
            s_AddressableAssetGroupText = new GUIContent("Addressable Group");
            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        private static void OnPostHeaderGUI(Editor editor)
        {
            if (editor.targets.Length > 1)
            {
                return;
            }

            var target = editor.target;

            DrawExportAsJsonField(target);
            DrawGroupField(target);

        }

        private static void DrawExportAsJsonField(UnityEngine.Object target)
        {
            bool hasSpaceBeenDrawn = false;

            using (new EditorGUILayout.HorizontalScope())
            {
                if (AssetEditorSettings.GetOrCreateSettings().showCopyGuidForObjects)
                {
                    EditorGUILayout.Space();
                    hasSpaceBeenDrawn = true;

                    if (GUILayout.Button("Copy GUID to Clipboard", GUILayout.ExpandWidth(false)))
                    {
                        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(target, out string guid, out long _);
                        GUIUtility.systemCopyBuffer = guid;
                    }
                }

                if (target is ScriptableObject && AssetEditorSettings.GetOrCreateSettings().showExportAsJsonForScriptableObjects)
                {
                    if (!hasSpaceBeenDrawn)
                    {
                        EditorGUILayout.Space();
                        hasSpaceBeenDrawn = true;
                    }

                    if (GUILayout.Button("Export as Json to Clipboard", GUILayout.ExpandWidth(false)))
                    {
                        string json = string.Empty;

                        if (target is ICustomiseJsonSerialization customSerialisation)
                        {
                            json = customSerialisation.Serialize();
                        }
                        else
                        {
                            json = JsonUtility.ToJson(target);
                        }

                        GUIUtility.systemCopyBuffer = json;
                    }
                }

                if (target is ScriptableObject && AssetEditorSettings.GetOrCreateSettings().showApplyJsonForScriptableObjects)
                {
                    if (!hasSpaceBeenDrawn)
                    {
                        EditorGUILayout.Space();
                        hasSpaceBeenDrawn = true;
                    }

                    using (new GUIEnabledScope(!string.IsNullOrEmpty(GUIUtility.systemCopyBuffer)))
                    {
                        if (GUILayout.Button("Apply Json from Clipboard", GUILayout.ExpandWidth(false)))
                        {
                            string json = GUIUtility.systemCopyBuffer;
                            
                            if (target is ICustomiseJsonSerialization customSerialisation)
                            {
                                customSerialisation.Deserialize(json);
                            }
                            else
                            {
                                JsonUtility.FromJsonOverwrite(json, target);
                            }

                            EditorUtility.SetDirty(target);
                        }
                    }
                }
            }
        }

        private static void DrawGroupField(UnityEngine.Object target)
        {
#if !USE_ADDRESSABLES
            return;
#else
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }      

            if (!AssetEditorSettings.GetOrCreateSettings().showAddressableGroupSelection)
            {
                return;
            }

            if (!target.IsAssetAddressable())
            {
                return;
            }

            var targetAddressableInfo = target.GetAddressableInfo();

            if (targetAddressableInfo == null)
            {
                return;
            }

            var aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            var groups = aaSettings.groups;
            string[] groupNames = new string[groups.Count];
            string selectedGroupName = targetAddressableInfo.parentGroup.Name;

            for (int i = 0, n = groupNames.Length; i < n; ++i)
            {
                groupNames[i] = groups[i].Name;
            }

            int selectedGroupIndex = Array.FindIndex(groupNames, x => string.CompareOrdinal(x, selectedGroupName) == 0);
            int newSelectedGroupIndex = EditorGUILayout.Popup(s_AddressableAssetGroupText, selectedGroupIndex, groupNames);

            if (selectedGroupIndex != newSelectedGroupIndex)
            {
                string newGroupName = groupNames[newSelectedGroupIndex];
                target.SetAddressableGroup(newGroupName);
                target.EnableOnlyAddressableLabel(newGroupName);
            }
#endif
        }
    }
}
