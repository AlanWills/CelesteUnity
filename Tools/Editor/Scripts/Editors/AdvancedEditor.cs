using Celeste.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace CelesteEditor.Tools
{
    public class AdvancedEditor : Editor
    {
        [NonSerialized] private Dictionary<string, Action<SerializedProperty>> propertyDrawCallbacks = new Dictionary<string, Action<SerializedProperty>>(StringComparer.Ordinal);

        private void OnEnable()
        {
            propertyDrawCallbacks.Clear();

            DoOnEnable();
        }

        private void OnDisable()
        {
            propertyDrawCallbacks.Clear();
        }

        public sealed override void OnInspectorGUI()
        {
            OnPrePropertiesGUI();

            serializedObject.Update();

            foreach (SerializedProperty property in serializedObject.EditorOnly_AllVisibleProperties())
            {
                DrawProperty(property);
            }

            serializedObject.ApplyModifiedProperties();

            OnPostPropertiesGUI();
        }

        protected virtual void DoOnEnable() { }
        protected virtual void OnPrePropertiesGUI() { }
        protected virtual void OnPostPropertiesGUI() { }

        protected void AddDrawPropertyCallback(string propertyName, Action<SerializedProperty> propertyDrawCallback)
        {
            propertyDrawCallbacks[propertyName] = propertyDrawCallback;
        }

        private void DrawProperty(SerializedProperty property)
        {
            if (propertyDrawCallbacks.TryGetValue(property.name, out var drawCallback))
            {
                drawCallback(property);
            }
            else
            {
                EditorGUILayout.PropertyField(property);
            }
        }
    }
}