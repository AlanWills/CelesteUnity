using Celeste.Localisation;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace CelesteEditor.Localisation.Catalogue
{
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor
    {
        #region Properties and Fields

        private int currentPage = 0;
        private List<(string, string)> localisationEntries = new List<(string, string)>();

        private const int ENTRIES_PER_PAGE = 40;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Language language = target as Language;
            
            localisationEntries.Clear();
            foreach (var entry in language.LocalisationLookup)
            {
                localisationEntries.Add(new ValueTuple<string, string>(entry.Key, entry.Value));
            }

            localisationEntries.Sort((a, b) =>
            {
                return string.Compare(a.Item1, b.Item1);
            });
        }

        public override void OnInspectorGUI()
        {
            Language language = target as Language;

            EditorGUILayout.LabelField("Num Keys", $"{language.NumLocalisationKeys}");
            EditorGUILayout.LabelField("Num Categories", $"{language.NumLocalisationKeyCategories}");

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            currentPage = GUIUtils.ReadOnlyPaginatedList(
                currentPage,
                ENTRIES_PER_PAGE,
                localisationEntries.Count,
                (i) =>
                {
                    var localisationEntry = localisationEntries[i];
                    EditorGUILayout.LabelField(localisationEntry.Item1, localisationEntry.Item2);
                });
        }

        #endregion
    }
}
