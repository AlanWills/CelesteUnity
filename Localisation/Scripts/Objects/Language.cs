using Celeste.Localisation.Catalogue;
using Celeste.Localisation.Pronouns;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(Language), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Language", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class Language : ScriptableObject, ISerializationCallbackReceiver
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

        #region Serialized Data

        [Serializable]
        private struct LocalisationData
        {
            public string key;
            public string localisedText;
            public AudioClip synthesizedSpeech;

            public LocalisationData(string key, string localisedText, AudioClip synthesizedSpeech)
            {
                this.key = key;
                this.localisedText = localisedText;
                this.synthesizedSpeech = synthesizedSpeech;
            }
        }

        [Serializable]
        private struct LocalisationCategoryData
        {
            public LocalisationKeyCategory category;
            public List<LocalisationKey> keys;

            public LocalisationCategoryData(LocalisationKeyCategory category, List<LocalisationKey> keys)
            {
                this.category = category;
                this.keys = new List<LocalisationKey>(keys);
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
        [SerializeField] private PronounFunctor pronounFunctor;
        [SerializeField] private List<LocalisationData> localisation = new List<LocalisationData>();
        [SerializeField] private List<LocalisationCategoryData> categories = new List<LocalisationCategoryData>();

        [NonSerialized] private Dictionary<string, string> localisationLookup = new Dictionary<string, string>();
        [NonSerialized] private Dictionary<string, AudioClip> speechLookup = new Dictionary<string, AudioClip>();
        [NonSerialized] private Dictionary<LocalisationKeyCategory, List<LocalisationKey>> categoryLookup = new Dictionary<LocalisationKeyCategory, List<LocalisationKey>>(new LocalisationKeyCategoryComparer());

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

        public string LocaliseAndRemoveAllPronouns(LocalisationKey key)
        {
            string localisedText = Localise(key);
            
            if (pronounFunctor != null)
            {
                localisedText = pronounFunctor.RemoveIndefinitePronouns(localisedText);
                localisedText = pronounFunctor.RemoveDefinitePronouns(localisedText);
            }

            return localisedText;
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

        public void AddEntries(IReadOnlyList<AudioClip> synthesizationEntries)
        {
            for (int i = 0, n = synthesizationEntries.Count; i < n; ++i)
            {
                AudioClip audioClip = synthesizationEntries[i];

                if (!speechLookup.ContainsKey(audioClip.name))
                {
                    speechLookup.Add(audioClip.name, audioClip);
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Speech lookup already contains key {audioClip.name}.");
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

        #region ISerializationCallbackReceiver

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            localisation.Clear();
            categories.Clear();

            foreach (var keyValuePair in localisationLookup)
            {
                speechLookup.TryGetValue(keyValuePair.Key, out AudioClip audioClip);
                localisation.Add(new LocalisationData(keyValuePair.Key, keyValuePair.Value, audioClip));
            }

            foreach (var keyValuePair in categoryLookup)
            {
                categories.Add(new LocalisationCategoryData(keyValuePair.Key, keyValuePair.Value));
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            localisationLookup.Clear();
            speechLookup.Clear();
            categoryLookup.Clear();

            foreach (LocalisationData localisationData in localisation)
            {
#if KEY_CHECKS
                UnityEngine.Debug.Assert(!localisationLookup.ContainsKey(localisationData.key), $"Duplicated localisation key {localisationData.key} found in localised text lookup in language {name}.");
#endif
                localisationLookup[localisationData.key] = localisationData.localisedText;

                if (localisationData.synthesizedSpeech != null)
                {
#if KEY_CHECKS
                    UnityEngine.Debug.Assert(!speechLookup.ContainsKey(localisationData.key), $"Duplicated localisation key {localisationData.key} found in speech lookup in language {name}.");
#endif
                    speechLookup[localisationData.key] = localisationData.synthesizedSpeech;
                }
            }

            foreach (LocalisationCategoryData localisationCategoryData in categories)
            {
#if KEY_CHECKS
                UnityEngine.Debug.Assert(!categoryLookup.ContainsKey(localisationCategoryData.category), $"Duplicated category {localisationCategoryData.category} found in category lookup in language {name}.");
#endif
                categoryLookup[localisationCategoryData.category] = localisationCategoryData.keys;
            }
        }

#endregion
    }
}
