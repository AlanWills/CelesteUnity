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

        [SerializeField] private string countryCode;
        [SerializeField] private bool assertOnFallback = true;
        [SerializeField] private List<LocalisationEntry> localisationEntries = new List<LocalisationEntry>();

        [NonSerialized] private Dictionary<LocalisationKey, string> localisationLookup = new Dictionary<LocalisationKey, string>();

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
            
            for (int i = 0, n = localisationEntries.Count; i < n; ++i)
            {
                var localisationEntry = localisationEntries[i];
                if (localisationEntry.key != null && !localisationLookup.ContainsKey(localisationEntry.key))
                {
                    localisationLookup.Add(localisationEntry.key, localisationEntry.localisedText);
                }
            }
        }
        
        #endregion
    }
}
