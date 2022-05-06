using UnityEditor;
using UnityEngine;
using Celeste.Web;
using System.Collections.Generic;
using Celeste.Localisation;
using CelesteEditor.Tools;
using CelesteEditor.Localisation.Utility;
using static Celeste.Localisation.Language;
using Celeste.Localisation.Settings;
using Celeste.Localisation.Tools;
using Celeste.Web.DataImporters;
using Celeste.Localisation.Catalogue;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(fileName = nameof(LocalisationDataImporter), menuName = "Celeste/Data Importers/Localisation Data Importer")]
    public class LocalisationDataImporter : GoogleSheetDataImporter
    {
        #region Properties and Fields

        [SerializeField] private LanguageCatalogue languageCatalogue;
        [SerializeField] private LocalisationKeyCatalogue localisationKeyCatalogue;
        [SerializeField] private LocalisationKeyCategoryCatalogue localisationKeyCategoryCatalogue;
        [SerializeField] private string localisationKeysDirectory = "Assets/Localisation/Data/Keys";
        [SerializeField] private string localisationKeyCategoriesDirectory = "Assets/Localisation/Data/Categories";

        #endregion

        protected override void OnDataReceived(GoogleSheet googleSheet)
        {
            GoogleSheet.Column keyStrings = googleSheet.GetColumn(0);
            GoogleSheet.Column categoryStrings = googleSheet.GetColumn(1);

            for (int column = LocalisationEditorSettings.GetOrCreateSettings().localisationSheetLanguagesOffset; column < googleSheet.NumColumns; ++column)
            {
                GoogleSheet.Column columnData = googleSheet.GetColumn(column);
                Language language = languageCatalogue.FindLanguageForTwoLetterCountryCode(columnData.Name);
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
                    LocalisationKey localisationKey = localisationKeyCatalogue.GetItem(keyString);

                    if (localisationKey == null)
                    {
                        localisationKey = CreateInstance<LocalisationKey>();
                        localisationKey.name = keyString.ToAssetName();
                        localisationKey.Key = keyString;
                        localisationKey.Fallback = localisedString;

                        Debug.Assert(!string.IsNullOrEmpty(keyString), $"No key found for row {row}.");
                        Debug.Assert(!string.IsNullOrEmpty(localisedString), $"No localised string found for row {row}.");
                        localisationKeyCatalogue.AddItem(keyString, localisationKey);

                        string directory = $"{localisationKeysDirectory}/{categoryString}";
                        AssetUtility.CreateAssetInFolder(localisationKey, directory);
                    }

                    LocalisationKeyCategory localisationKeyCategory = localisationKeyCategoryCatalogue.FindByCategoryName(categoryString);

                    if (localisationKeyCategory == null)
                    {
                        localisationKeyCategory = CreateInstance<LocalisationKeyCategory>();
                        localisationKeyCategory.name = categoryString.ToAssetName();
                        localisationKeyCategory.CategoryName = categoryString;

                        localisationKeyCategoryCatalogue.AddItem(localisationKeyCategory);

                        AssetUtility.CreateAssetInFolder(localisationKeyCategory, localisationKeyCategoriesDirectory);
                    }
                    
                    localisationKey.Category = localisationKeyCategory;
                    localisationEntries.Add(new LocalisationEntry(localisationKey, localisedString));
                }

                language.AddEntries(localisationEntries);
                EditorUtility.SetDirty(language);
                AssetDatabase.SaveAssets();
            }

            LocalisationPostImportContext context = new LocalisationPostImportContext()
            {
                googleSheet = googleSheet,
                localisationKeyLookup = localisationKeyCatalogue.Items
            };

            var postImportSteps = LocalisationEditorSettings.GetOrCreateSettings().postImportSteps;
            for (int i = 0, n = postImportSteps != null ? postImportSteps.Count : 0; i < n; ++i)
            {
                Debug.Log($"Executing post import step '{postImportSteps[i].name}'.");
                postImportSteps[i].Execute(context);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Localisation Data Importing Done!");
        }
    }
}