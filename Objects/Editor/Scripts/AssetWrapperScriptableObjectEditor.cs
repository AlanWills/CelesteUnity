using System;
using System.Collections.Generic;
using Celeste.Objects;
using Celeste.Tools;
using CelesteEditor.Tools.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CelesteEditor.Objects
{
    public class AssetWrapperScriptableObjectEditor<T> : Editor where T : Object
    {
        private int selectedTypeIndex = -1;
        private static bool typesFound;
        private static readonly List<Type> assetTypes = new();
        private static readonly List<string> assetDisplayNames = new();
        
        public override void OnInspectorGUI()
        {
            AssetWrapperScriptableObject<T> assetWrapper = target as AssetWrapperScriptableObject<T>;

            if (!typesFound)
            {
                RefreshChoices();
            }
            
            if (assetWrapper.asset == null)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    selectedTypeIndex =
                        EditorGUILayout.Popup("Node Type", selectedTypeIndex, assetDisplayNames.ToArray());

                    if (GUILayout.Button("Refresh Choices", GUILayout.ExpandWidth(false)))
                    {
                        RefreshChoices();
                    }
                }

                if (selectedTypeIndex >= 0)
                {
                    assetWrapper.asset = CreateInstance(assetTypes[selectedTypeIndex]) as T;
                    assetWrapper.asset.hideFlags = HideFlags.None;
                    assetWrapper.asset.name = assetDisplayNames[selectedTypeIndex];
                    assetWrapper.asset.AddObjectToAsset(assetWrapper);
                }
            }
            else
            {
                Editor assetEditor = CreateEditor(assetWrapper.asset);
                assetEditor.OnInspectorGUI();
            }
        }

        private void RefreshChoices()
        {
            typesFound = true;
            TypeExtensions.LoadTypes<T>(assetTypes, assetDisplayNames);
        }
    }
}