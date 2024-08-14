using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
#if USE_ADDRESSABLES
using UnityEditor.AddressableAssets;
#endif
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AddressableGroupAttribute : MultiPropertyAttribute, IGUIAttribute
    {
#if UNITY_EDITOR
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
#if USE_ADDRESSABLES
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                EditorGUI.LabelField(position, label, new GUIContent("No Addressable Settings exists!"));
            }
            else
            {
                var addressableGroupNames = AddressableAssetSettingsDefaultObject.Settings.groups.Select(x => x.Name).ToArray();
                int currentIndex = Array.IndexOf(addressableGroupNames, property.stringValue);
                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, addressableGroupNames);

                if (newIndex != currentIndex)
                {
                    property.stringValue = newIndex < 0 ? string.Empty : addressableGroupNames[newIndex];
                }
            }
#endif
            return position;
        }
#endif
        }
}