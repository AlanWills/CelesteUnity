using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation.Catalogue
{
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor
    {
        #region Properties and Fields

        private int currentPage = 0;
        private List<(string, string)> localisationEntries = new List<(string, string)>();
        private SerializedProperty localisationKeyCatalogueProperty;

        private const int ENTRIES_PER_PAGE = 40;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Language language = target as Language;
            
            localisationEntries.Clear();
            for (int i = 0, n = language.NumLocalisationKeys; i < n; ++i)
            {
                localisationEntries.Add(language.GetLocalisationEntry(i));
            }

            localisationEntries.Sort((a, b) =>
            {
                return string.Compare(a.Item1, b.Item1);
            });

            localisationKeyCatalogueProperty = serializedObject.FindProperty("localisationKeyCatalogue");
        }

        public override void OnInspectorGUI()
        {
            Language language = target as Language;

            EditorGUILayout.LabelField("Num Keys", $"{language.NumLocalisationKeys}");
            EditorGUILayout.LabelField("Num Categories", $"{language.NumLocalisationKeyCategories}");
            EditorGUILayout.LabelField("Num Speech", $"{language.NumLocalisationSpeech}");

            using (new GUIEnabledScope(localisationKeyCatalogueProperty.objectReferenceValue != null))
            {
                if (GUILayout.Button("Add Entries From Localisation Key Catalogue"))
                {
                    LocalisationKeyCatalogue localisationKeyCatalogue = localisationKeyCatalogueProperty.objectReferenceValue as LocalisationKeyCatalogue;

                    language.ClearEntries();
                    language.AddEntries(localisationKeyCatalogue.Items.Values.Select(x => new Language.LocalisationEntry()
                    {
                        key = x,
                        localisedText = x.Fallback
                    }).ToList());
                }
            }

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            currentPage = GUIExtensions.ReadOnlyPaginatedList(
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
