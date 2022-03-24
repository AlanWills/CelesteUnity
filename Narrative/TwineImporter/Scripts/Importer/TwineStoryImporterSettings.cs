using Celeste.DataStructures;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Celeste.Narrative.TwineImporter.ParserSteps;
using Celeste.FSM;
using Celeste.Twine;

namespace Celeste.Narrative.TwineImporter
{
    [CreateAssetMenu(fileName = nameof(TwineStoryImporterSettings), menuName = "Celeste/Twine/Twine Story Importer Settings")]
    public class TwineStoryImporterSettings : ScriptableObject
    {
        #region Properties and Fields

        public string CharactersDirectory
        {
            get { return Path.Combine(rootDirectory, charactersDirectory); }
        }

        public string LocaTokensDirectory
        {
            get { return Path.Combine(rootDirectory, locaTokensDirectory); }
        }

        public string ConditionsDirectory
        {
            get { return Path.Combine(rootDirectory, conditionsDirectory); }
        }

        public string ParametersDirectory
        {
            get { return Path.Combine(rootDirectory, parametersDirectory); }
        }

        public string BackgroundsDirectory
        {
            get { return Path.Combine(rootDirectory, backgroundsDirectory); }
        }

        public string SubNarrativesDirectory
        {
            get { return Path.Combine(rootDirectory, subNarrativesDirectory); }
        }

        public string InventoryItemsDirectory
        {
            get { return Path.Combine(rootDirectory, inventoryItemsDirectory); }
        }

        public string AudioClipsDirectory
        {
            get { return Path.Combine(rootDirectory, audioClipsDirectory); }
        }

        [SerializeField] private TwineNodeParserStep[] parserSteps;

        [Header("Parsing")]
        [SerializeField] private string linkStartDelimiter = "[[";
        [SerializeField] private string linkEndDelimiter = "]]";

        [Header("Tags")]
        [SerializeField] private string ignoreTag = "Ignore";
        [SerializeField] private string startTag = "Start";
        
        [Header("Graph Settings")]
        public Vector2 nodeSpread = new Vector2(2, 2);

        [Header("File System Settings")]
        [SerializeField] private string rootDirectory = "Assets";
        [SerializeField] private string charactersDirectory = "Characters";
        [SerializeField] private string locaTokensDirectory = "LocaTokens";
        [SerializeField] private string conditionsDirectory = "Conditions";
        [SerializeField] private string parametersDirectory = "Parameters";
        [SerializeField] private string backgroundsDirectory = "Backgrounds";
        [SerializeField] private string subNarrativesDirectory = "SubNarratives";
        [SerializeField] private string inventoryItemsDirectory = "InventoryItems";
        [SerializeField] private string audioClipsDirectory = "Audio";

        #endregion

        #region Analysis

        public void Analyse(TwineStory twineStory, TwineStoryAnalysis twineStoryAnalysis)
        {
            twineStoryAnalysis.Dispose();

            TwineNodeAnalyseContext analyseContext = new TwineNodeAnalyseContext()
            {
                ImporterSettings = this,
                Analysis = twineStoryAnalysis,
            };

            foreach (TwineNode twineNode in twineStory.passages)
            {
                analyseContext.StopAnalysing = false;
                analyseContext.TwineNode = twineNode;

                IEnumerable<string> tags = twineNode.Tags;

                for (int i = 0, n = parserSteps != null ? parserSteps.Length : 0; i < n && !analyseContext.StopAnalysing; ++i)
                {
                    var parserStep = parserSteps[i];

                    if (parserStep.CanAnalyse(analyseContext))
                    {
                        parserStep.Analyse(analyseContext);
                    }

                    foreach (string tag in tags)
                    {
                        if (!IsRecognizedTag(tag))
                        {
                            twineStoryAnalysis.unrecognizedTags.Add(tag);
                        }
                    }
                }
            }
        }

        #endregion

        #region Parsing

        public bool TryParse(
            TwineNode twineNode,
            FSMGraph graph,
            Vector2 startingNodePosition,
            out FSMNode output)
        {
            TwineNodeParseContext parseContext = new TwineNodeParseContext()
            {
                ImporterSettings = this,
                TwineNode = twineNode,
                Graph = graph,
                StartingNodePosition = startingNodePosition
            };

            for (int i = 0, n = parserSteps.Length; i < n && !parseContext.StopParsing; ++i)
            {
                if (parserSteps[i].CanParse(parseContext))
                {
                    parserSteps[i].Parse(parseContext);
                }
            }
            
            output = parseContext.FSMNode;
            return output != null;
        }

        public string StripLinksFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            int delimiterStart = text.IndexOf(linkStartDelimiter);

            while (delimiterStart != -1)
            {
                int delimiterEnd = text.IndexOf(linkEndDelimiter, delimiterStart + 2);
                text = text.Remove(delimiterStart, delimiterEnd - delimiterStart + 2);
                delimiterStart = text.IndexOf(linkStartDelimiter, delimiterStart);
            }

            return text.Trim();
        }

        #endregion

        #region Tag Utility

        public bool ContainsIgnoreTag(IList<string> tags)
        {
            return ContainsTag(tags, ignoreTag);
        }

        public bool ContainsStartTag(IList<string> tags)
        {
            return ContainsTag(tags, startTag);
        }

        private bool ContainsTag(IList<string> tags, string desiredTag)
        {
            return tags.Exists((string s) => string.CompareOrdinal(s, desiredTag) == 0);
        }

        public bool IsRecognizedTag(string tag)
        {
            if (string.CompareOrdinal(tag, ignoreTag) == 0)
            {
                return true;
            }

            if (string.CompareOrdinal(tag, startTag) == 0)
            {
                return true;
            }

            foreach (var parserStep in parserSteps)
            {
                if (parserStep is IUsesTags && (parserStep as IUsesTags).UsesTag(tag))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Key Utility

        public void AddKey<T>(string key, T obj)
        {
            bool keyAdded = false;

            for (int i = 0, n = parserSteps != null ? parserSteps.Length : 0; i < n; i++)
            {
                var parserStep = parserSteps[i];

                if (parserStep is IUsesKeys<T>)
                {
                    (parserStep as IUsesKeys<T>).AddKey(key, obj);
                    keyAdded = true;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(parserStep);
#endif
                }
            }

            UnityEngine.Debug.Assert(keyAdded, $"Could not find parser step which uses key {key} for object {obj}.");
        }

        #endregion
    }
}