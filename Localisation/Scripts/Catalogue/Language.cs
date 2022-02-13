using Celeste.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(Language), menuName = "Celeste/Localisation/Language")]
    public class Language : ScriptableObject
    {
        #region LocalisationEntry

        [Serializable]
        public struct LocalisationEntry
        {
            public LocalisationKey key;
            public string localisedText;

            public LocalisationEntry(LocalisationKey key, string localisedText)
            {
                this.key = key;
                this.localisedText = localisedText;
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

        #region Properties and Fields

        public string CountryCode => countryCode;
        public int NumEntries => localisationEntries.Count;
        public LocalisationKey LanguageNameKey => languageNameKey;

        [SerializeField] private string countryCode;
        [SerializeField] private LocalisationKey languageNameKey;
        [SerializeField] private bool assertOnFallback = true;
        [SerializeField] private List<LocalisationEntry> localisationEntries = new List<LocalisationEntry>();

        [NonSerialized] private Dictionary<LocalisationKey, string> localisationLookup = new Dictionary<LocalisationKey, string>(new LocalisationKeyComparer());
        [NonSerialized] private Dictionary<LocalisationKeyCategory, List<LocalisationKey>> categoryLookup = new Dictionary<LocalisationKeyCategory, List<LocalisationKey>>();

        #endregion

        public void Initialize()
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
                    else
                    {
                        UnityEngine.Debug.LogAssertion($"No category set for localisation key {localisationKey}.");
                    }
                }
            }
        }

        public string Localise(LocalisationKey key)
        {
            if (key == null)
            {
                UnityEngine.Debug.LogAssertion("Failed to perform localisation due to null inputted key.");
                return string.Empty;
            }

            if (!localisationLookup.TryGetValue(key, out string localisedText))
            {
                UnityEngine.Debug.Assert(!assertOnFallback, $"Failed to localise '{key.Key}' due to missing entry.  Using fallback...");
                return key.Fallback;
            }

            return localisedText;
        }

        public void SetEntries(List<LocalisationEntry> entries)
        {
            localisationEntries.Clear();
            localisationEntries.AddRange(entries);

            Initialize();
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
    }
}
