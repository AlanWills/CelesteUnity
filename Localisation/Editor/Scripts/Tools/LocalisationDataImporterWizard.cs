using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using Celeste.Web;
using System;
using System.Collections.Generic;
using Celeste.Localisation;
using CelesteEditor.Tools;
using System.IO;
using System.Globalization;
using CelesteEditor.Localisation.Utility;
using static Celeste.Localisation.Language;

namespace CelesteEditor.Localisation.Tools
{
    public class LocalisationDataImporterWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField] private string sheetId;
        [SerializeField] private string localisationKeysDirectory = "Assets/Localisation/Data/Keys";

        private const int LANGUAGES_OFFSET = 2;
        private const string LOCALISATION_SHEET_ID = "LocalisationSheetId";

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Localisation Data Importer")]
        public static void ShowLocalisationDataImporterWizard()
        {
            DisplayWizard<LocalisationDataImporterWizard>("Localisation Data Importer", "Close", "Import");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            sheetId = EditorPrefs.GetString(LOCALISATION_SHEET_ID);
        }

        protected override bool DrawWizardGUI()
        {
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                sheetId = EditorGUILayout.TextField("Sheet Id", sheetId);
                
                if (changeCheck.changed)
                {
                    EditorPrefs.SetString(LOCALISATION_SHEET_ID, sheetId);
                }

                localisationKeysDirectory = EditorGUILayout.TextField("Localisation Keys Directory", localisationKeysDirectory);

                return changeCheck.changed;
            }
        }

        private void OnWizardOtherButton()
        {
            EditorCoroutineUtility.StartCoroutine(GoogleSheetsCSVDownloader.DownloadData(sheetId, OnDownloadData), this);
        }

        private void OnWizardCreate()
        {
            Close();
        }

        #endregion

        #region Callbacks

        private void OnDownloadData(string data)
        {
            GoogleSheet googleSheet = GoogleSheet.FromCSV(data);
            List<Language> languagesToLocalise = AssetUtility.FindAssets<Language>();
            Dictionary<string, LocalisationKeyCategory> localisationKeyCategories = new Dictionary<string, LocalisationKeyCategory>();
            Dictionary<string, LocalisationKey> localisationKeys = new Dictionary<string, LocalisationKey>();

            for (int i = languagesToLocalise.Count - 1; i >= 0; --i)
            {
                if (!googleSheet.HasColumn(languagesToLocalise[i].CountryCode))
                {
                    languagesToLocalise.RemoveAt(i);
                }
            }

            foreach (LocalisationKeyCategory localisationKeyCategory in AssetUtility.FindAssets<LocalisationKeyCategory>())
            {
                localisationKeyCategories.Add(localisationKeyCategory.CategoryName, localisationKeyCategory);
            }

            foreach (LocalisationKey localisationKey in AssetUtility.FindAssets<LocalisationKey>())
            {
                localisationKeys.Add(localisationKey.Key, localisationKey);
            }

            GoogleSheet.Column keyStrings = googleSheet.GetColumn(0);
            GoogleSheet.Column categoryStrings = googleSheet.GetColumn(1);

            for (int column = LANGUAGES_OFFSET; column < googleSheet.NumColumns; ++column)
            {
                GoogleSheet.Column columnData = googleSheet.GetColumn(column);
                Language language = languagesToLocalise.Find(x => string.CompareOrdinal(x.CountryCode, columnData.Name) == 0);
                List<LocalisationEntry> localisationEntries = new List<LocalisationEntry>();

                for (int row = 0, n = keyStrings.Values.Count; row < n; ++row)
                {
                    string keyString = keyStrings.Values[row];
                    string localisedString = columnData.Values[row];
                    string categoryString = categoryStrings.Values[row];

                    if (string.IsNullOrEmpty(localisedString))
                    {
                        Debug.LogAssertion($"Key {keyString} has no localised string set for language {language.name}.");
                        continue;
                    }

                    // Need to create a new localisation key asset
                    if (!localisationKeys.TryGetValue(keyString, out LocalisationKey key))
                    {
                        key = CreateInstance<LocalisationKey>();
                        key.name = keyString.ToAssetName();
                        key.Key = keyString;
                        key.Fallback = localisedString;

                        Debug.Assert(!string.IsNullOrEmpty(keyString), $"No key found for row {row}.");
                        Debug.Assert(!string.IsNullOrEmpty(localisedString), $"No localised string found for row {row}.");
                        localisationKeys.Add(keyString, key);

                        string directory = $"{localisationKeysDirectory}/{categoryString}";
                        AssetUtility.CreateAssetInFolder(key, directory);
                    }
                    
                    if (localisationKeyCategories.TryGetValue(categoryString, out LocalisationKeyCategory category))
                    {
                        key.Category = category;
                    }
                    else
                    {
                        Debug.LogError($"Could not find {nameof(LocalisationKeyCategory)} '{categoryString}' for {keyString}.");
                    }

                    localisationEntries.Add(new LocalisationEntry(key, localisedString));
                }

                language.SetEntries(localisationEntries);
                EditorUtility.SetDirty(language);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Localisation Data Importing Done!");
        }

        #endregion
    }
}