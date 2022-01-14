using Celeste.Localisation;
using Celeste.Tools;
using CelesteEditor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace CelesteEditor.Localisation
{
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor
    {
        #region Properties and Fields

        private SerializedProperty localisationEntriesProperty;

        private bool useFallbackWhenAddingMissing = false;
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

            useFallbackWhenAddingMissing = EditorGUILayout.Toggle("Use Fallback Text", useFallbackWhenAddingMissing);

            if (Button("Add Missing"))
            {
                AddMissingKeysFrom(AssetUtility.FindAssets<LocalisationKey>());
            }

            if (Button("Add Missing From Folder Recursive"))
            {
                AddMissingKeysFrom(AssetUtility.FindAssets<LocalisationKey>(AssetUtility.GetAssetFolderPath(target)));
            }

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

        private void AddMissingKeysFrom(List<LocalisationKey> foundKeys)
        {
            Language language = target as Language;

            HashSet<LocalisationKey> keys = new HashSet<LocalisationKey>();
            for (int i = 0, n = language.NumEntries; i < n; ++i)
            {
                keys.Add(language.GetKey(i));
            }

            List<LocalisationKey> missingKeys = new List<LocalisationKey>();
            foreach (LocalisationKey key in foundKeys)
            {
                if (!keys.Contains(key))
                {
                    missingKeys.Add(key);
                }
            }

            if (missingKeys.Count > 0)
            {
                language.RemoveNullEntries();
                language.AddEntries(missingKeys, useFallbackWhenAddingMissing);

                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}