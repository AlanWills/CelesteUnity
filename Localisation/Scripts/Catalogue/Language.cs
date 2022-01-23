using Celeste.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(Language), menuName = "Celeste/Localisation/Language")]
    public class Language : ScriptableObject, ISerializationCallbackReceiver
    {
        #region LocalisationEntry

        [Serializable]
        private struct LocalisationEntry
        {
            public LocalisationKey key;
            public string localisedText;
        }

        #endregion

        #region Properties and Fields

        public string CountryCode => countryCode;
        public int NumEntries => localisationEntries.Count;
        public LocalisationKey LanguageNameKey => languageNameKey;

        [SerializeField] private string countryCode;
        [SerializeField] private LocalisationKey languageNameKey;
        [SerializeField] private bool assertOnFallback = true;
        [SerializeField] private List<LocalisationEntry> localisationEntries = new List<LocalisationEntry>();

        [NonSerialized] private Dictionary<LocalisationKey, string> localisationLookup = new Dictionary<LocalisationKey, string>();
        [NonSerialized] private Dictionary<LocalisationKeyCategory, List<LocalisationKey>> categoryLookup = new Dictionary<LocalisationKeyCategory, List<LocalisationKey>>();

        #endregion

        public string Localise(LocalisationKey key)
        {
            if (key == null)
            {
                UnityEngine.Debug.LogAssertion("Failed to perform localisation due to null inputted key.");
                return string.Empty;
            }

            if (!localisationLookup.TryGetValue(key, out string localisedValue))
            {
                UnityEngine.Debug.Assert(!assertOnFallback, $"Failed to localise '{key.Key}' due to missing entry.  Using fallback...");
                return key.Fallback;
            }

            return localisedValue;
        }

        public void AddEntries(List<LocalisationKey> keys, bool useFallbackForText)
        {
            for (int i = 0, n = keys.Count; i < n; ++i)
            {
                LocalisationKey key = keys[i];
                if (!localisationLookup.ContainsKey(key))
                {
                    string localisedText = useFallbackForText ? key.Fallback : string.Empty;
                    localisationLookup.Add(key, localisedText);
                }
                else
                {
                    UnityEngine.Debug.LogError($"Attempting to add key {key} which already exists.  Ignoring...");
                }
            }
        }

        public void AddOrUpdateEntry(LocalisationKey key, string localisedText)
        {
            if (!localisationLookup.ContainsKey(key))
            {
                localisationLookup.Add(key, localisedText);
            }
            else
            {
                localisationLookup[key] = localisedText;
            }
        }

        public LocalisationKey GetKey(int index)
        {
            return localisationEntries.Get(index).key;
        }

        public int NumEntriesInCategory(LocalisationKeyCategory category)
        {
            if (categoryLookup.TryGetValue(category, out List<LocalisationKey> value))
            {
                return value.Count;
            }

            return 0;
        }

        public LocalisationKey GetRandomKey(LocalisationKeyCategory category)
        {
            if (categoryLookup.TryGetValue(category, out List<LocalisationKey> value))
            {
                return value[UnityEngine.Random.Range(0, value.Count)];
            }

            return null;
        }

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize() 
        {
            localisationEntries.Clear();

            foreach (var localisationEntry in localisationLookup)
            {
                localisationEntries.Add(new LocalisationEntry() { key = localisationEntry.Key, localisedText = localisationEntry.Value });
            }
        }

        public void OnAfterDeserialize()
        {
            localisationLookup.Clear();
            categoryLookup.Clear();

            for (int i = 0, n = localisationEntries.Count; i < n; ++i)
            {
                var localisationEntry = localisationEntries[i];
                var localisationKey = localisationEntry.key;

                if (localisationKey != null)
                {
                    if (!localisationLookup.ContainsKey(localisationKey))
                    {
                        localisationLookup.Add(localisationKey, localisationEntry.localisedText);
                    }

                    if (localisationKey.Category != null)
                    {
                        if (!categoryLookup.TryGetValue(localisationKey.Category, out List<LocalisationKey> list))
                        {
                            list = new List<LocalisationKey>();
                            categoryLookup.Add(localisationKey.Category, list);
                        }

                        list.Add(localisationKey);
                    }
                }
            }
        }

        #endregion
    }
}
