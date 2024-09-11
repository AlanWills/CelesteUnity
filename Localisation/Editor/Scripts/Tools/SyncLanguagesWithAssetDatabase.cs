using Celeste;
using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using Celeste.Tools;
using Celeste.Web;
using Celeste.Web.ImportSteps;
using CelesteEditor.Localisation.Utility;
using CelesteEditor.Tools;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(
        fileName = nameof(SyncLanguagesWithAssetDatabase),
        menuName = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM + "Sync Languages With Asset Database",
        order = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM_PRIORITY)]
    public class SyncLanguagesWithAssetDatabase : GoogleSheetReceivedImportStep
    {
        #region Properties and Fields

        [Header("Sheet Structure")]
        [SerializeField] private int keyColumn = 0;
        [SerializeField] private int categoryColumn = 1;
        [SerializeField] private int languagesColumnOffset = 2;

        [Header("Data")]
        [SerializeField] private LanguageCatalogue languageCatalogue;
        [SerializeField] private LocalisationKeyCatalogue localisationKeyCatalogue;
        [SerializeField] private LocalisationKeyCategoryCatalogue localisationKeyCatalogueCatalogue;
        [SerializeField] private string localisationKeysDirectory = "Assets/Localisation/Data/Keys";
        [SerializeField] private string localisationKeyCategoriesDirectory = "Assets/Localisation/Data/Categories";
        [SerializeField] private string audioDirectory = "Assets/Localisation/Audio";

        #endregion

        public override void Execute(GoogleSheet googleSheet)
        {
            using (new AssetEditingScope())
            {
                GoogleSheet.Column keyStrings = googleSheet.GetColumn(keyColumn);
                GoogleSheet.Column categoryStrings = googleSheet.GetColumn(categoryColumn);
                Dictionary<string, LocalisationKey> localisationKeyLookup = AssetDatabaseExtensions.CreateAssetNameLookup<LocalisationKey>();
                Dictionary<string, LocalisationKeyCategory> localisationKeyCategoryLookup = AssetDatabaseExtensions.CreateAssetNameLookup<LocalisationKeyCategory>();

                for (int column = languagesColumnOffset; column < googleSheet.NumColumns; ++column)
                {
                    GoogleSheet.Column columnData = googleSheet.GetColumn(column);
                    Language language = languageCatalogue.FindLanguageForTwoLetterCountryCode(columnData.Name);
                    Debug.Assert(language != null, $"Could not find language for country code {columnData.Name}.");

                    string languageAudioDirectory = Path.Combine(audioDirectory, language.CountryCode);
                    Dictionary<string, AudioClip> audioLookup = AssetDatabaseExtensions.CreateAssetNameLookup<AudioClip>(languageAudioDirectory);

                    List<Language.LocalisationData> languageLocalisationData = new List<Language.LocalisationData>();
                    List<Language.LocalisationCategoryData> languageLocalisationCategoryData = new List<Language.LocalisationCategoryData>();
                    List<Language.LocalisationSpeechData> languageSpeechData = new List<Language.LocalisationSpeechData>();

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

                        string localisationKeyName = keyString.ToAssetName();
                        string localisationCategoryName = $"{categoryString.ToAssetName()}Category";

                        if (!localisationKeyLookup.TryGetValue(localisationKeyName, out LocalisationKey localisationKey))
                        {
                            Debug.LogAssertion($"Key {localisationKeyName} could not be found in the Asset Database.  Skipping loc key {keyString}...");
                            continue;
                        }
                        else
                        {
                            languageLocalisationData.Add(new Language.LocalisationData(keyString, localisedString));
                        }

                        if (!localisationKeyCategoryLookup.TryGetValue(localisationCategoryName, out LocalisationKeyCategory localisationKeyCategory))
                        {
                            Debug.LogAssertion($"Category {localisationCategoryName} could not be found in the Asset Database.  Skipping loc key {keyString}...");
                            continue;
                        }
                        else
                        {
                            int categoryDataIndex = languageLocalisationCategoryData.FindIndex(x => x.category == localisationKeyCategory);

                            if (categoryDataIndex < 0)
                            {
                                languageLocalisationCategoryData.Add(new Language.LocalisationCategoryData(localisationKeyCategory, new List<LocalisationKey>()
                                {
                                    localisationKey
                                }));
                            }
                            else
                            {
                                var categoryData = languageLocalisationCategoryData[categoryDataIndex];
                                categoryData.keys.Add(localisationKey);
                                languageLocalisationCategoryData[categoryDataIndex] = categoryData;
                            }
                        }

                        if (!audioLookup.TryGetValue(keyString, out AudioClip localisedSpeech))
                        {
                            Debug.Log($"Audio {keyString} could not be found in the Asset Database.  Synthesized speech will not be added...");
                            continue;
                        }
                        else
                        {
                            languageSpeechData.Add(new Language.LocalisationSpeechData(keyString, localisedSpeech));
                        }

                        localisationKey.Category = localisationKeyCategory;
                    }

                    language.AddData(languageLocalisationData);
                    language.AddData(languageLocalisationCategoryData);
                    language.AddData(languageSpeechData);
                }
            }
        }
    }
}