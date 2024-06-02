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
        }

        private void OnDisable()
        {
            propertyDrawCallbacks.Clear();
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnPrePropertiesGUI();

            SerializedProperty serializedProperty = serializedObject.GetIterator();

            DrawProperty(serializedProperty);

            foreach (SerializedProperty property in serializedProperty.EditorOnly_VisibleChildProperties())
            {
                DrawProperty(property);
            }

            OnPostPropertiesGUI();

            serializedObject.ApplyModifiedProperties();
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