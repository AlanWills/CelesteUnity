using UnityEngine;
using System;
using UnityEditor.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AddressableLabelAttribute : MultiPropertyAttribute, IGUIAttribute
    {
#if UNITY_EDITOR
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                EditorGUI.LabelField(position, label, new GUIContent("No Addressable Settings exists!"));
            }
            else
            {
                var addressableLabels = AddressableAssetSettingsDefaultObject.Settings.GetLabels();
                int currentIndex = addressableLabels.IndexOf(property.stringValue);
                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, addressableLabels.ToArray());

                if (newIndex != currentIndex)
                {
                    property.stringValue = newIndex < 0 ? string.Empty : addressableLabels[newIndex];
                }
            }

            return position;
        }
#endif
    }
}