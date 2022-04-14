using System;
using System.Collections.Generic;

namespace Celeste.Narrative.TwineImporter
{
    public class TwineStoryAnalysis : IDisposable
    {
        #region Properties and Fields
        public IReadOnlyCollection<string> FoundCharacters => foundCharacters;
        public IReadOnlyCollection<string> FoundLocaTokens => foundLocaTokens;
        public IReadOnlyCollection<string> FoundConditions => foundConditions;
        public IReadOnlyCollection<string> FoundParameters => foundParameters;
        public IReadOnlyCollection<string> FoundBackgrounds => foundBackgrounds;
        public IReadOnlyCollection<string> FoundSubNarratives => foundSubNarratives;
        public IReadOnlyCollection<string> FoundInventoryItems => foundInventoryItems;
        public IReadOnlyCollection<string> FoundSFXs => foundSFXs;
        public IReadOnlyCollection<string> UnrecognizedTags => unrecognizedTags;
        public IReadOnlyCollection<string> UnrecognizedKeys => unrecognizedKeys;

        private HashSet<string> foundCharacters = new HashSet<string>();
        private HashSet<string> foundLocaTokens = new HashSet<string>();
        private HashSet<string> foundConditions = new HashSet<string>();
        private HashSet<string> foundParameters = new HashSet<string>();
        private HashSet<string> foundBackgrounds = new HashSet<string>();
        private HashSet<string> foundSubNarratives = new HashSet<string>();
        private HashSet<string> foundInventoryItems = new HashSet<string>();
        private HashSet<string> foundSFXs = new HashSet<string>();
        private HashSet<string> unrecognizedTags = new HashSet<string>();
        private HashSet<string> unrecognizedKeys = new HashSet<string>();

        #endregion

        public void AddFoundCharacter(string foundCharacter)
        {
            foundCharacters.Add(foundCharacter);
        }

        public void AddFoundLocaToken(string foundLocaToken)
        {
            foundLocaTokens.Add(foundLocaToken);
        }

        public void AddFoundConditions(string foundCondition)
        {
            foundConditions.Add(foundCondition);
        }

        public void AddFoundParameter(string foundParameter)
        {
            foundParameters.Add(foundParameter);
        }

        public void AddFoundBackground(string foundBackground)
        {
            foundBackgrounds.Add(foundBackground);
        }

        public void AddFoundInventoryItem(string foundInventoryItem)
        {
            foundInventoryItems.Add(foundInventoryItem);
        }

        public void AddFoundSFXs(string foundSFX)
        {
            foundSFXs.Add(foundSFX);
        }

        public void AddFoundSubNarrative(string foundSubNarrative)
        {
            foundSubNarratives.Add(foundSubNarrative);
        }

        public void AddUnrecognizedTag(string unrecognizedTag)
        {
            unrecognizedTags.Add(unrecognizedTag);
        }

        public void RemoveUnrecognizedTag(string unrecognizedTag)
        {
            unrecognizedTags.Remove(unrecognizedTag);
        }

        public void AddUnrecognizedKey(string unrecognizedKey)
        {
            unrecognizedKeys.Add(unrecognizedKey);
        }

        public void RemoveUnrecognizedKey(string unrecognizedKey)
        {
            unrecognizedKeys.Remove(unrecognizedKey);
        }

        public void Dispose()
        {
            foundCharacters.Clear();
            foundLocaTokens.Clear();
            foundConditions.Clear();
            foundParameters.Clear();
            foundBackgrounds.Clear();
            foundSubNarratives.Clear();
            foundInventoryItems.Clear();
            foundSFXs.Clear();
            unrecognizedTags.Clear();
            unrecognizedKeys.Clear();
        }
    }
}