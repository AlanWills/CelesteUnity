using Celeste;
using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using Celeste.Tools;
using Celeste.Web;
using Celeste.Web.ImportSteps;
using CelesteEditor.Localisation.Utility;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(
        fileName = nameof(CreateLocalisationKeysAndCategories), 
        menuName = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM + "Create Localisation Keys And Categories",
        order = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM_PRIORITY)]
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
        [SerializeField] private string localisationKeysDirectory = "Assets/Localisation/Data/Keys";
        [SerializeField] private string localisationKeyCategoriesDirectory = "Assets/Localisation/Data/Categories";

        #endregion

        public override void Execute(GoogleSheet googleSheet)
        {
            using (new AssetEditingScope())
            {
                GoogleSheet.Column keyStrings = googleSheet.GetColumn(keyColumn);
                GoogleSheet.Column categoryStrings = googleSheet.GetColumn(categoryColumn);
                Dictionary<string, LocalisationKey> localisationKeyLookup = AssetDatabaseExtensions.CreateAssetNameLookup<LocalisationKey>(localisationKeysDirectory);
                Dictionary<string, LocalisationKeyCategory> localisationKeyCategoryLookup = AssetDatabaseExtensions.CreateAssetNameLookup<LocalisationKeyCategory>(localisationKeyCategoriesDirectory);

                for (int column = languagesColumnOffset; column < googleSheet.NumColumns; ++column)
                {
                    GoogleSheet.Column columnData = googleSheet.GetColumn(column);
                    Language language = languageCatalogue.FindLanguageForTwoLetterCountryCode(columnData.Name);
                    Debug.Assert(language != null, $"Could not find language for country code {columnData.Name}.");

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

                        string localisationKeyName = keyString.ToAssetName();
                        string localisationCategoryName = $"{categoryString}Category";

                        if (!localisationKeyLookup.TryGetValue(localisationKeyName, out LocalisationKey localisationKey))
                        {
                            Debug.Assert(!string.IsNullOrEmpty(keyString), $"Null or empty key string found for column {column} in row {row} for category {categoryString}.");
                            Debug.Assert(!string.IsNullOrEmpty(localisedString), $"No localised string found for column {column} in row {row} for category {categoryString}.");
                            localisationKey = CreateInstance<LocalisationKey>();
                            localisationKey.name = localisationKeyName;
                            localisationKey.Key = keyString;
                            localisationKey.Fallback = localisedString;

                            string directory = $"{localisationKeysDirectory}/{categoryString}";
                            EditorOnly.CreateAssetInFolder(localisationKey, directory);

                            localisationKeyLookup.Add(localisationKeyName, localisationKey);
                        }

                        if (!localisationKeyCategoryLookup.TryGetValue(localisationCategoryName, out LocalisationKeyCategory localisationKeyCategory))
                        {
                            localisationKeyCategory = CreateInstance<LocalisationKeyCategory>();
                            localisationKeyCategory.name = localisationCategoryName;
                            localisationKeyCategory.CategoryName = categoryString;

                            EditorOnly.CreateAssetInFolder(localisationKeyCategory, localisationKeyCategoriesDirectory);

                            localisationKeyCategoryLookup.Add(localisationCategoryName, localisationKeyCategory);
                        }
                    }
                }
            }
        }
    }
}