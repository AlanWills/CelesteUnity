using Celeste.Localisation.Catalogue;
using Celeste.OdinSerializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(Language), menuName = "Celeste/Localisation/Language")]
    public class Language : SerializedScriptableObject
    {
        #region LocalisationEntry

        [Serializable]
        public struct LocalisationEntry
        {
            public LocalisationKey key;
            public string localisedText;
            public AudioClip synthesizedSpeech;

            public LocalisationEntry(LocalisationKey key, string localisedText, AudioClip synthesizedSpeech)
            {
                this.key = key;
                this.localisedText = localisedText;
                this.synthesizedSpeech = synthesizedSpeech;
            }
        }

        #endregion

        #region Localisation Key Comparer

        private struct LocalisationKeyComparer : IEqualityComparer<LocalisationKey>
        {
            public bool Equals(LocalisationKey x, LocalisationKey y)
            {
                if (x == null || y == null)
                {
                    return x == null && y == null;
                }

                return string.CompareOrdinal(x.Key, y.Key) == 0;
            }

            public int GetHashCode(LocalisationKey obj)
            {
                return !string.IsNullOrEmpty(obj.Key) ? obj.Key.GetHashCode() : 0;
            }
        }

        #endregion

        #region Localisation Key Category Comparer

        private class LocalisationKeyCategoryComparer : IEqualityComparer<LocalisationKeyCategory>
        {
            public bool Equals(LocalisationKeyCategory x, LocalisationKeyCategory y)
            {
                if (x == null || y == null)
                {
                    return x == null && y == null;
                }

                return string.CompareOrdinal(x.CategoryName, y.CategoryName) == 0;
            }

            public int GetHashCode(LocalisationKeyCategory obj)
            {
                return !string.IsNullOrEmpty(obj.CategoryName) ? obj.CategoryName.GetHashCode() : 0;
            }
        }

        #endregion

        #region Properties and Fields

        public string CountryCode => countryCode;
        public LocalisationKey LanguageNameKey => languageNameKey;
        public Sprite LanguageIcon => languageIcon;
        public int NumLocalisationKeys => localisationLookup.Count;
        public int NumLocalisationKeyCategories => categoryLookup.Count;
        public int NumLocalisationSpeech => speechLookup.Count;
        public IReadOnlyDictionary<string, string> LocalisationLookup => localisationLookup;
        public IReadOnlyDictionary<LocalisationKeyCategory, List<LocalisationKey>> CategoryLookup => categoryLookup;

        [SerializeField] private string countryCode;
        [SerializeField] private LocalisationKey languageNameKey;
        [SerializeField] private Sprite languageIcon;
        [SerializeField] private bool assertOnFallback = true;
        [SerializeField] private LocalisationKeyCatalogue localisationKeyCatalogue;
        [SerializeField] private NumberToLocalisedTextConverter numberToTextConverter;
        [SerializeField] private Dictionary<string, string> localisationLookup = new Dictionary<string, string>();
        [SerializeField] private Dictionary<string, AudioClip> speechLookup = new Dictionary<string, AudioClip>();
        [SerializeField] private Dictionary<LocalisationKeyCategory, List<LocalisationKey>> categoryLookup = new Dictionary<LocalisationKeyCategory, List<LocalisationKey>>(new LocalisationKeyCategoryComparer());

        #endregion

        public string Localise(LocalisationKey key)
        {
            if (key == null)
            {
                UnityEngine.Debug.LogAssertion("Failed to perform localisation due to null inputted key.  No fallback possible...");
                return string.Empty;
            }

            return Localise(key.Key, key.Fallback);
        }

        public string Localise(string key, string fallback = "")
        {
            if (!localisationLookup.TryGetValue(key, out string localisedText))
            {
                UnityEngine.Debug.Assert(!assertOnFallback, $"Failed to localise '{key}' due to missing entry.");
                return fallback;
            }

            return localisedText;
        }

        public string Localise(int number)
        {
            if (numberToTextConverter == null)
            {
                UnityEngine.Debug.LogAssertion($"Failed to localise {number} due to missing converter.  No fallback possible...");
                return number.ToString();
            }

            return numberToTextConverter.Localise(number, this);
        }

        public string Truncate(int number)
        {
            return numberToTextConverter != null ? numberToTextConverter.Truncate(number, this) : number.ToString();
        }

        public AudioClip Synthesize(LocalisationKey key)
        {
            if (key == null)
            {
                UnityEngine.Debug.LogAssertion("Failed to perform synthesize due to null inputted key.  No fallback possible...");
                return null;
            }

            if (!speechLookup.TryGetValue(key.Key, out AudioClip synthesizedText))
            {
                UnityEngine.Debug.Assert(!assertOnFallback, $"Failed to perform synthesize of '{key}' due to missing entry.  No fallback possible...");
                return null;
            }

            return synthesizedText;
        }

        public void AddEntries(List<LocalisationEntry> localisationEntries)
        {
            for (int i = 0, n = localisationEntries.Count; i < n; ++i)
            {
                var localisationEntry = localisationEntries[i];
                var localisationKey = localisationEntry.key;

                if (localisationKey != null)
                {
                    // Add to text localisation lookup
                    if (!localisationLookup.ContainsKey(localisationKey.Key))
                    {
                        localisationLookup.Add(localisationKey.Key, localisationEntry.localisedText);
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion($"Localisation lookup already contains key {localisationKey.Key} ({localisationKey.name}).");
                    }

                    // Add to category lookup
                    if (localisationKey.Category != null)
                    {
                        if (!categoryLookup.TryGetValue(localisationKey.Category, out List<LocalisationKey> list))
                        {
                            list = new List<LocalisationKey>();
                            categoryLookup.Add(localisationKey.Category, list);
                        }

                        list.Add(localisationKey);
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion($"No category set for localisation key {localisationKey}.");
                    }

                    // Add to audio lookup
                    if (localisationEntry.synthesizedSpeech != null)
                    {
                        if (!speechLookup.ContainsKey(localisationKey.Key))
                        {
                            speechLookup.Add(localisationKey.Key, localisationEntry.synthesizedSpeech);
                        }
                        else
                        {
                            UnityEngine.Debug.LogAssertion($"Speech lookup already contains key {localisationKey.Key} ({localisationKey.name}).");
                        }
                    }
                }
            }
        }

        public void ClearEntries()
        {
            localisationLookup.Clear();
            categoryLookup.Clear();
            speechLookup.Clear();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public bool HasKey(LocalisationKey localisationKey)
        {
            return localisationLookup.ContainsKey(localisationKey.Key);
        }

        public LocalisationKey FindKey(string key)
        {
            return localisationKeyCatalogue != null ? localisationKeyCatalogue.GetItem(key) : null;
        }

        public bool CanSynthesize(LocalisationKey key)
        {
            return speechLookup.ContainsKey(key.Key);
        }

        public int NumEntriesInCategory(LocalisationKeyCategory category)
        {
            if (categoryLookup.TryGetValue(category, out List<LocalisationKey> value))
            {
                return value.Count;
            }

            return 0;
        }

        public string GetRandomWord(LocalisationKeyCategory category)
        {
            if (categoryLookup.TryGetValue(category, out List<LocalisationKey> value))
            {
                return Localise(value[UnityEngine.Random.Range(0, value.Count)]);
            }

            UnityEngine.Debug.LogAssertion($"Could not find category {category.name} in Language {name}.");
            return string.Empty;
        }

        public ReadOnlyCollection<LocalisationKey> GetKeysForCategory(LocalisationKeyCategory category)
        {
            return new ReadOnlyCollection<LocalisationKey>(categoryLookup.TryGetValue(category, out List<LocalisationKey> value) ? value : new List<LocalisationKey>());
        }
    }
}
