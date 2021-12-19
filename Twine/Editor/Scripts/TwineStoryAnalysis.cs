using System;
using System.Collections.Generic;

namespace CelesteEditor.Twine
{
    public class TwineStoryAnalysis : IDisposable
    {
        public HashSet<string> foundCharacters = new HashSet<string>();
        public HashSet<string> foundLocaTokens = new HashSet<string>();
        public HashSet<string> foundConditions = new HashSet<string>();
        public HashSet<string> foundParameters = new HashSet<string>();
        public HashSet<string> foundBackgrounds = new HashSet<string>();
        public HashSet<string> foundSubNarratives = new HashSet<string>();
        public HashSet<string> foundInventoryItems = new HashSet<string>();
        public HashSet<string> unrecognizedTags = new HashSet<string>();
        public HashSet<string> unrecognizedKeys = new HashSet<string>();

        public void Dispose()
        {
            foundCharacters.Clear();
            foundLocaTokens.Clear();
            foundConditions.Clear();
            foundParameters.Clear();
            foundBackgrounds.Clear();
            foundSubNarratives.Clear();
            foundInventoryItems.Clear();
            unrecognizedTags.Clear();
            unrecognizedKeys.Clear();
        }
    }
}