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

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            localisationLookup.Clear();
            
            for (int i = 0, n = localisationEntries.Count; i < n; ++i)
            {
                localisationLookup.Add(localisationEntries[i].key, localisationEntries[i].localisedText);
            }
        }
        
        #endregion
    }
}
