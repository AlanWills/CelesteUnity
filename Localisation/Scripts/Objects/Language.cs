using Celeste.DataStructures;
using Celeste.Localisation.Catalogue;
using Celeste.Localisation.Pronouns;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(Language), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Language", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class Language : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Serialized Data

        [Serializable]
        public struct LocalisationData
        {
            public string key;
            public string localisedText;

            public LocalisationData(string key, string localisedText)
            {
                this.key = key;
                this.localisedText = localisedText;
            }
        }

        [Serializable]
        public struct LocalisationCategoryData
        {
            public LocalisationKeyCategory category;
            public List<LocalisationKey> keys;

            public LocalisationCategoryData(LocalisationKeyCategory category, List<LocalisationKey> keys)
            {
                this.category = category;
                this.keys = new List<LocalisationKey>(keys);
            }
        }

        [Serializable]
        public struct LocalisationSpeechData
        {
            public string key;
            public AudioClip synthesizedSpeech;

            public LocalisationSpeechData(string key, AudioClip synthesizedSpeech)
            {
                this.key = key;
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
        public string DefaultVoiceName => defaultVoiceName;
        public int NumLocalisationKeys => localisation.Count;
        public int NumLocalisationKeyCategories => categories.Count;
        public int NumLocalisationSpeech => speech.Count;

        private IReadOnlyDictionary<string, string> LocalisationLookup
        {
            get
            {
                if (_localisationLookup.Count != localisation.Count)
                {
                    RebuildLocalisationLookup();
                }

                return _localisationLookup;
            }
        }

        private IReadOnlyDictionary<string, AudioClip> SpeechLookup
        {
            get
            {
                if (_speechLookup.Count != speech.Count)
                {
                    RebuildSpeechLookup();
                }

                return _speechLookup;
            }
        }

        private IReadOnlyDictionary<LocalisationKeyCategory, List<LocalisationKey>> CategoryLookup
        {
            get
            {
                if (_categoryLookup.Count != categories.Count)
                {
                    RebuildCategoryLookup();
                }

                return _categoryLookup;
            }
        }

        [SerializeField] private string countryCode;
        [SerializeField] private LocalisationKey languageNameKey;
        [SerializeField] private Sprite languageIcon;
        [SerializeField] private string defaultVoiceName;
        [SerializeField] private bool assertOnFallback = true;
        [SerializeField] private LocalisationKeyCatalogue localisationKeyCatalogue;
        [SerializeField] private NumberToLocalisedTextConverter numberToTextConverter;
        [SerializeField] private PronounFunctor pronounFunctor;
        [SerializeField] private List<LocalisationData> localisation = new List<LocalisationData>();
        [SerializeField] private List<LocalisationCategoryData> categories = new List<LocalisationCategoryData>();
        [SerializeField] private List<LocalisationSpeechData> speech = new List<LocalisationSpeechData>();

        [NonSerialized] private Dictionary<string, string> _localisationLookup = new Dictionary<string, string>(StringComparer.Ordinal);
        [NonSerialized] private Dictionary<LocalisationKeyCategory, List<LocalisationKey>> _categoryLookup = new Dictionary<LocalisationKeyCategory, List<LocalisationKey>>(new LocalisationKeyCategoryComparer());
        [NonSerialized] private Dictionary<string, AudioClip> _speechLookup = new Dictionary<string, AudioClip>(StringComparer.Ordinal);

        #endregion

        public string Localise(LocalisationKey key)
        {
            if (key == null)
            {
                UnityEngine.Debug.LogAssertion($"Failed to perform localisation due to null inputted key in language {name}.  No fallback possible...");
                return string.Empty;
            }

            return Localise(key.Key, key.Fallback);
        }

        public string Localise(string key, string fallback = "")
        {
            if (!LocalisationLookup.TryGetValue(key, out string localisedText))
            {
                UnityEngine.Debug.Assert(!assertOnFallback, $"Failed to localise '{key}' due to missing entry in language {name}.");
                return fallback;
            }

            return localisedText;
        }

        public string Localise(int number)
        {
            if (numberToTextConverter == null)
            {
                UnityEngine.Debug.LogAssertion($"Failed to localise {number} due to missing converter in language {name}.  No fallback possible...");
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

        public bool CanSynthesize(LocalisationKey key)
        {
            return key != null && SpeechLookup.TryGetValue(key.Key, out AudioClip synthesizedText) && synthesizedText != null;
        }

        public AudioClip Synthesize(LocalisationKey key)
        {
            if (key == null)
            {
                UnityEngine.Debug.LogAssertion($"Failed to perform synthesize due to null inputted key in language {name}.  No fallback possible...");
                return null;
            }

            if (!SpeechLookup.TryGetValue(key.Key, out AudioClip synthesizedText))
            {
                UnityEngine.Debug.Assert(!assertOnFallback, $"Failed to perform synthesize of '{key}' due to missing entry in language {name}.  No fallback possible...");
                return null;
            }

            return synthesizedText;
        }

        public void AddData(IReadOnlyList<LocalisationData> localisationData)
        {
            for (int i = 0, n = localisationData.Count; i < n; ++i)
            {
                LocalisationData data = localisationData[i];

                // Add to text localisation lookup
                if (!LocalisationLookup.ContainsKey(data.key))
                {
                    localisation.Add(new LocalisationData(data.key, data.localisedText));
                    _localisationLookup[data.key] = data.localisedText;
                    EditorOnly.SetDirty(this);
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Localisation lookup already contains key {data.key} ({data.localisedText}).");
                }
            }
        }

        public void AddData(IReadOnlyList<LocalisationCategoryData> categoryData)
        {
            for (int i = 0, n = categoryData.Count; i < n; ++i)
            {
                LocalisationCategoryData data = categoryData[i];

                if (!CategoryLookup.TryGetValue(data.category, out List<LocalisationKey> list))
                {
                    list = new List<LocalisationKey>(data.keys);
                    categories.Add(new LocalisationCategoryData(data.category, list));
                    _categoryLookup.Add(data.category, list);
                    EditorOnly.SetDirty(this);
                }
                else
                {
                    list.AddRange(data.keys);
                }
            }
        }

        public void AddData(IReadOnlyList<LocalisationSpeechData> speechData)
        {
            for (int i = 0, n = speechData.Count; i < n; ++i)
            {
                LocalisationSpeechData data = speechData[i];

                if (!SpeechLookup.ContainsKey(data.key))
                {
                    speech.Add(new LocalisationSpeechData(data.key, data.synthesizedSpeech));
                    _speechLookup.Add(data.key, data.synthesizedSpeech);
                    EditorOnly.SetDirty(this);
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Speech lookup already contains key {data.key}.");
                }
            }
        }

        public void MergeData(IReadOnlyList<LocalisationSpeechData> speechData)
        {
            for (int i = 0, n = speechData.Count; i < n; ++i)
            {
                LocalisationSpeechData data = speechData[i];
                int existingIndex = speech.FindIndex(x => string.CompareOrdinal(x.key, data.key) == 0);

                if (existingIndex < 0)
                {
                    speech.Add(new LocalisationSpeechData(data.key, data.synthesizedSpeech));
                }
                else
                {
                    LocalisationSpeechData existingData = speech[existingIndex];
                    existingData.synthesizedSpeech = data.synthesizedSpeech;
                    speech[existingIndex] = existingData;
                }
                
                _speechLookup[data.key] = data.synthesizedSpeech;
                EditorOnly.SetDirty(this);
            }
        }

        public void ClearData()
        {
            localisation.Clear();
            categories.Clear();
            speech.Clear();
            _localisationLookup.Clear();
            _categoryLookup.Clear();
            _speechLookup.Clear();

            EditorOnly.SetDirty(this);
        }

        public ValueTuple<string, string> GetLocalisationEntry(int index)
        {
            var localisationData = localisation.Get(index);
            return new (localisationData.key, localisationData.localisedText);
        }

        public bool HasKey(LocalisationKey localisationKey)
        {
            return LocalisationLookup.ContainsKey(localisationKey.Key);
        }

        public LocalisationKey FindKey(string key)
        {
            return localisationKeyCatalogue != null ? localisationKeyCatalogue.GetItem(key) : null;
        }

        public int NumEntriesInCategory(LocalisationKeyCategory category)
        {
            if (CategoryLookup.TryGetValue(category, out List<LocalisationKey> value))
            {
                return value.Count;
            }

            return 0;
        }

        public string GetRandomWord(LocalisationKeyCategory category)
        {
            if (CategoryLookup.TryGetValue(category, out List<LocalisationKey> value))
            {
                return Localise(value[UnityEngine.Random.Range(0, value.Count)]);
            }

            UnityEngine.Debug.LogAssertion($"Could not find category {category.name} in Language {name}.");
            return string.Empty;
        }

        public ReadOnlyCollection<LocalisationKey> GetKeysForCategory(LocalisationKeyCategory category)
        {
            return new ReadOnlyCollection<LocalisationKey>(CategoryLookup.TryGetValue(category, out List<LocalisationKey> value) ? value : new List<LocalisationKey>());
        }

        private void RebuildLocalisationLookup()
        {
            _localisationLookup.Clear();

            foreach (LocalisationData localisationData in localisation)
            {
#if KEY_CHECKS
                UnityEngine.Debug.Assert(!_localisationLookup.ContainsKey(localisationData.key), $"Duplicated localisation key {localisationData.key} found in localised text lookup in language {name}.");
#endif
                _localisationLookup[localisationData.key] = localisationData.localisedText;
            }
        }

        private void RebuildCategoryLookup()
        {
            _categoryLookup.Clear();

            foreach (LocalisationCategoryData localisationCategoryData in categories)
            {
#if KEY_CHECKS
                if (_categoryLookup.ContainsKey(localisationCategoryData.category))
                {
                    UnityEngine.Debug.LogAssertion($"Duplicated category {localisationCategoryData.category.CategoryName} found in category lookup in language {name}.");
                }
#endif
                _categoryLookup[localisationCategoryData.category] = localisationCategoryData.keys;
            }
        }

        private void RebuildSpeechLookup()
        {
            _speechLookup.Clear();

            foreach (LocalisationSpeechData speechData in speech)
            {
#if KEY_CHECKS
                UnityEngine.Debug.Assert(!_speechLookup.ContainsKey(speechData.key), $"Duplicated localisation key {speechData.key} found in speech lookup in language {name}.");
#endif
                _speechLookup[speechData.key] = speechData.synthesizedSpeech;
            }
        }

        #region ISerializationCallbackReceiver

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _localisationLookup.Clear();
            _speechLookup.Clear();
            _categoryLookup.Clear();
        }

        #endregion
    }
}
