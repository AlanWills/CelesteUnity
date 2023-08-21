using CelesteEditor.Tools;
using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace CelesteEditor.Assets.GUI
{
    [InitializeOnLoad]
    internal static class AssetHeaderGUI
    {
        private static GUIContent addressableAssetGroupText;

        static AssetHeaderGUI()
        {
            addressableAssetGroupText = new GUIContent("Addressable Group");
            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        private static void OnPostHeaderGUI(Editor editor)
        {
            if (editor.targets.Length > 1)
            {
                return;
            }

            var target = editor.target;

            if (GUILayout.Button("Export as Json to Clipboard", GUILayout.ExpandWidth(false)))
            {
                string json = JsonUtility.ToJson(target);
                GUIUtility.systemCopyBuffer = json;
            }

            if (target.IsAssetAddressable())
            {
                DrawGroupField(target);
            }
        }

        private static void DrawGroupField(UnityEngine.Object target)
        {
            var targetAddressableInfo = target.GetAddressableInfo();
            var aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            var groups = aaSettings.groups;
            string[] groupNames = new string[groups.Count];
            string selectedGroupName = targetAddressableInfo.parentGroup.Name;

            for (int i = 0, n = groupNames.Length; i < n; ++i)
            {
                groupNames[i] = groups[i].Name;
            }

            int selectedGroupIndex = Array.FindIndex(groupNames, x => string.CompareOrdinal(x, selectedGroupName) == 0);
            int newSelectedGroupIndex = EditorGUILayout.Popup(addressableAssetGroupText, selectedGroupIndex, groupNames);

            if (selectedGroupIndex != newSelectedGroupIndex)
            {
                string newGroupName = groupNames[newSelectedGroupIndex];
                target.SetAddressableGroup(newGroupName);
                target.EnableOnlyAddressableLabel(newGroupName);
            }
        }
    }
}
