using UnityEditor;
using UnityEngine;
using Celeste.Web;
using System.Collections.Generic;
using Celeste.Localisation;
using CelesteEditor.Tools;
using CelesteEditor.Localisation.Utility;
using static Celeste.Localisation.Language;
using Celeste.Localisation.Catalogue;
using Celeste.Web.ImportSteps;
using System.IO;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(fileName = nameof(CreateLocalisationKeysAndCategories), menuName = "Celeste/Data Importers/Create Localisation Keys And Categories")]
    public class CreateLocalisationKeysAndCategories : GoogleSheetReceivedImportStep
    {
        #region Properties and Fields

        [Header("Sheet Structure")]
        [SerializeField] private int keyColumn = 0;
        [SerializeField] private int categoryColumn = 1;
        [SerializeField] private int languagesColumnOffset = 2;

        [Header("Validation")]
        [SerializeField] private List<char> disallowedTrailingCharacters = new List<char>() { '?' };

        [Header("Data")]
        [SerializeField] private LanguageCatalogue languageCatalogue;
        [SerializeField] private LocalisationKeyCatalogue localisationKeyCatalogue;
        [SerializeField] private LocalisationKeyCategoryCatalogue localisationKeyCategoryCatalogue;
        [SerializeField] private string localisationKeysDirectory = "Assets/Localisation/Data/Keys";
        [SerializeField] private string localisationKeyCategoriesDirectory = "Assets/Localisation/Data/Categories";
        [SerializeField] private string localisationAudioDirectory = "Assets/Localisation/Audio";

        #endregion

        public override void Execute(GoogleSheet googleSheet)
        {
            GoogleSheet.Column keyStrings = googleSheet.GetColumn(keyColumn);
            GoogleSheet.Column categoryStrings = googleSheet.GetColumn(categoryColumn);

            for (int column = languagesColumnOffset; column < googleSheet.NumColumns; ++column)
            {
                GoogleSheet.Column columnData = googleSheet.GetColumn(column);
                Language language = languageCatalogue.FindLanguageForTwoLetterCountryCode(columnData.Name);
                Debug.Assert(language != null, $"Could not find language for country code {columnData.Name}.");

                Dictionary<string, AudioClip> speechLookup = CreateSpeechLookup(language);
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

                    foreach (char disallowedTrailingCharacters in disallowedTrailingCharacters)
                    {
                        if (localisedString.EndsWith(disallowedTrailingCharacters))
                        {
                            Debug.LogError($"{keyString} {language.CountryCode} localisation ends with disallowed character {disallowedTrailingCharacters}.  Removing...");
                            localisedString = localisedString.Remove(localisedString.Length - 1);
                            break;
                        }
                    }

                    if (string.CompareOrdinal(keyString, "GOODNIGHT") == 0)
                    {
                        Debug.Log("Got here");
                    }

                    // Need to create a new localisation key asset
                    LocalisationKey localisationKey = localisationKeyCatalogue.GetItem(keyString);

                    if (localisationKey == null)
                    {
                        Debug.Assert(!string.IsNullOrEmpty(keyString), $"Null or empty key string found for column {column} in row {row} for category {categoryString}.");
                        Debug.Assert(!string.IsNullOrEmpty(localisedString), $"No localised string found for column {column} in row {row} for category {categoryString}.");
                        localisationKey = CreateInstance<LocalisationKey>();
                        localisationKey.name = keyString.ToAssetName();
                        localisationKey.Key = keyString;
                        localisationKey.Fallback = localisedString;

                        localisationKeyCatalogue.AddItem(keyString, localisationKey);

                        string directory = $"{localisationKeysDirectory}/{categoryString}";
                        AssetUtility.CreateAssetInFolder(localisationKey, directory);
                    }

                    LocalisationKeyCategory localisationKeyCategory = localisationKeyCategoryCatalogue.FindByCategoryName(categoryString);

                    if (localisationKeyCategory == null)
                    {
                        localisationKeyCategory = CreateInstance<LocalisationKeyCategory>();
                        localisationKeyCategory.name = $"{categoryString.ToAssetName()}Category";
                        localisationKeyCategory.CategoryName = categoryString;

                        localisationKeyCategoryCatalogue.AddItem(localisationKeyCategory);

                        AssetUtility.CreateAssetInFolder(localisationKeyCategory, localisationKeyCategoriesDirectory);
                    }

                    if (!speechLookup.TryGetValue(localisationKey.Key, out AudioClip audioClip))
                    {
                        Debug.LogWarning($"Could not find audio clip for localisation key {localisationKey.Key} and language {language.name}.");
                    }
                    
                    localisationKey.Category = localisationKeyCategory;
                    localisationEntries.Add(new LocalisationEntry(localisationKey, localisedString, audioClip));
                }

                language.AddEntries(localisationEntries);
                EditorUtility.SetDirty(language);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private Dictionary<string, AudioClip> CreateSpeechLookup(Language language)
        {
            Dictionary<string, AudioClip> speechLookup = new Dictionary<string, AudioClip>();
            string audioDirectory = Path.Combine(localisationAudioDirectory, language.CountryCode);

            if (Directory.Exists(audioDirectory))
            {
                foreach (AudioClip audioClip in AssetUtility.FindAssets<AudioClip>("", audioDirectory))
                {
                    speechLookup.Add(audioClip.name, audioClip);
                }
            }

            return speechLookup;
        }
    }
}