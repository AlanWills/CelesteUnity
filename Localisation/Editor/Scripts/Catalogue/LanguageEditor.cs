using Celeste.Localisation;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation
{
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor
    {
        #region Properties and Fields

        private SerializedProperty localisationEntriesProperty;

        private int currentPage = 0;
        private int ENTRIES_PER_PAGE = 40;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            localisationEntriesProperty = serializedObject.FindProperty("localisationEntries");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "localisationEntries");

            EditorGUILayout.Space();

            currentPage = GUIUtils.PaginatedList(
                currentPage,
                ENTRIES_PER_PAGE,
                localisationEntriesProperty.arraySize,
                DrawLocalisationEntry,
                AddLocalisationEntry,
                RemoveLocalisationEntry);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLocalisationEntry(int index)
        {
            SerializedProperty localisationEntry = localisationEntriesProperty.GetArrayElementAtIndex(index);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                SerializedProperty keyProperty = localisationEntry.FindPropertyRelative("key");
                SerializedProperty localisedTextProperty = localisationEntry.FindPropertyRelative("localisedText");

                EditorGUILayout.PropertyField(keyProperty, GUIContent.none);
                EditorGUILayout.PropertyField(localisedTextProperty, GUIContent.none);
            }
        }

        private void AddLocalisationEntry()
        {
            int oldSize = localisationEntriesProperty.arraySize;
            ++localisationEntriesProperty.arraySize;

            var localisationEntry = localisationEntriesProperty.GetArrayElementAtIndex(oldSize);
            localisationEntry.FindPropertyRelative("key").objectReferenceValue = null;
        }

        private void RemoveLocalisationEntry(int index)
        {
            localisationEntriesProperty.DeleteArrayElementAtIndex(index);
        }

        #endregion
    }
}