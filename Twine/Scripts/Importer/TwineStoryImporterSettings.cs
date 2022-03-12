using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Celeste.Narrative.UI;
using Celeste.Narrative.Characters;
using Celeste.Narrative;
using Celeste.Logic;
using Celeste.Inventory;
using Celeste.Twine.ParserSteps;
using Celeste.Twine.AnalysisSteps;
using Celeste.FSM;

namespace Celeste.Twine
{
    [CreateAssetMenu(fileName = nameof(TwineStoryImporterSettings), menuName = "Celeste/Twine/Twine Story Importer Settings")]
    public class TwineStoryImporterSettings : ScriptableObject
    {
        #region UIPosition Tag Struct

        [Serializable]
        public struct UIPositionTag
        {
            public string tag;
            public UIPosition position;
        }

        #endregion

        #region DialogueType Tag Struct

        [Serializable]
        public struct DialogueTypeTag
        {
            public string tag;
            public DialogueType dialogueType;
        }

        #endregion

        #region Character Tag Struct

        [Serializable]
        public struct CharacterTag
        {
            public string tag;
            public Character character;

            public CharacterTag(string tag, Character character)
            {
                this.tag = tag;
                this.character = character;
            }
        }

        #endregion

        #region Loca Token Key Struct

        [Serializable]
        public struct LocaTokenKey
        {
            public string key;
            public ScriptableObject token;

            public LocaTokenKey(string key, ScriptableObject token)
            {
                this.key = key;
                this.token = token;
            }
        }

        #endregion

        #region Condition Key Struct

        [Serializable]
        public struct ConditionKey
        {
            public string key;
            public Condition condition;

            public ConditionKey(string key, Condition condition)
            {
                this.key = key;
                this.condition = condition;
            }
        }

        #endregion

        #region Parameter Key Struct

        [Serializable]
        public struct ParameterKey
        {
            public string key;
            public ScriptableObject parameter;

            public ParameterKey(string key, ScriptableObject parameter)
            {
                this.key = key;
                this.parameter = parameter;
            }
        }

        #endregion

        #region Background Key Struct

        [Serializable]
        public struct BackgroundKey
        {
            public string key;
            public Background background;

            public BackgroundKey(string key, Background background)
            {
                this.key = key;
                this.background = background;
            }
        }

        #endregion

        #region Sub Narrative Struct

        [Serializable]
        public struct SubNarrativeKey
        {
            public string key;
            public NarrativeGraph subNarrative;

            public SubNarrativeKey(string key, NarrativeGraph subNarrative)
            {
                this.key = key;
                this.subNarrative = subNarrative;
            }
        }

        #endregion

        #region Inventory Item Key Struct

        [Serializable]
        public struct InventoryItemKey
        {
            public string key;
            public InventoryItem inventoryItem;

            public InventoryItemKey(string key, InventoryItem inventoryItem)
            {
                this.key = key;
                this.inventoryItem = inventoryItem;
            }
        }

        #endregion

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

        [SerializeField] private TwineNodeParserStep[] parserSteps;
        [SerializeField] private TwineNodeAnalysisStep[] analysisSteps;

        [Header("Parsing")]
        [SerializeField] private char locaTokenStartDelimiter = '{';
        [SerializeField] private char locaTokenEndDelimiter = '}';
        [SerializeField] private char conditionStartDelimiter = '#';
        [SerializeField] private char conditionEndDelimiter = '#';
        [SerializeField] private char parameterStartDelimiter = '$';
        [SerializeField] private char parameterEndDelimiter = '$';
        [SerializeField] private string linkStartDelimiter = "[[";
        [SerializeField] private string linkEndDelimiter = "]]";

        [Header("Tags")]
        [SerializeField] private string ignoreTag = "Ignore";
        [SerializeField] private string startTag = "Start";
        [SerializeField] private string finishTag = "Finish";
        [SerializeField] private string dialogueTag = "Dialogue";
        [SerializeField] private string choiceTag = "Choice";
        [SerializeField] private string scriptTag = "Script";
        [SerializeField] private string branchTag = "Branch";
        [SerializeField] private UIPositionTag[] uiPositionTags;
        [SerializeField] private DialogueTypeTag[] dialogueTypeTags;
        public List<CharacterTag> characterTags = new List<CharacterTag>();

        [Header("Keys")]
        public List<LocaTokenKey> locaTokenKeys = new List<LocaTokenKey>();
        public List<ConditionKey> conditionKeys = new List<ConditionKey>();
        public List<ParameterKey> parameterKeys = new List<ParameterKey>();
        public List<BackgroundKey> backgroundKeys = new List<BackgroundKey>();
        public List<SubNarrativeKey> subNarrativeKeys = new List<SubNarrativeKey>();
        public List<InventoryItemKey> inventoryItemKeys = new List<InventoryItemKey>();

        [Header("Script Instructions")]
        [SerializeField] private string setParameterInstruction = "SetParameter";
        [SerializeField] private string setBackgroundInstruction = "SetBackground";
        [SerializeField] private string subNarrativeInstruction = "SubNarrative";
        [SerializeField] private string addInventoryItemInstruction = "AddInventoryItem";

        [Header("Branch Instructions")]
        [SerializeField] private string randomBranch = "Random";

        [Header("Events")]
        public Events.Event finishEvent;
        public Events.BackgroundEvent setBackgroundEvent;
        public Events.InventoryItemEvent addInventoryItemEvent;

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

                for (int i = 0, n = analysisSteps != null ? analysisSteps.Length : 0; i < n && !analyseContext.StopAnalysing; ++i)
                {
                    if (analysisSteps[i].CanAnalyse(analyseContext))
                    {
                        analysisSteps[i].Analyse(analyseContext);
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

        public Character FindCharacterInTags(IList<string> tags)
        {
            for (int i = 0, n = tags != null ? tags.Count : 0; i < n; ++i)
            {
                var characterTagIndex = characterTags.FindIndex(x => string.CompareOrdinal(x.tag, tags[i]) == 0);
                if (characterTagIndex != -1)
                {
                    return characterTags[characterTagIndex].character;
                }
            }

            return null;
        }

        public UIPosition FindUIPositionFromTag(IList<string> tags, UIPosition fallbackValue)
        {
            for (int i = 0, n = tags != null ? tags.Count : 0; i < n; ++i)
            {
                var positionTagIndex = uiPositionTags.FindIndex(x => string.CompareOrdinal(x.tag, tags[i]) == 0);
                if (positionTagIndex != -1)
                {
                    return uiPositionTags[positionTagIndex].position;
                }
            }

            return fallbackValue;
        }

        public DialogueType FindDialogueTypeFromTag(IList<string> tags, DialogueType fallbackValue)
        {
            for (int i = 0, n = tags != null ? tags.Count : 0; i < n; ++i)
            {
                var dialogueTypeTagIndex = dialogueTypeTags.FindIndex(x => string.CompareOrdinal(x.tag, tags[i]) == 0);
                if (dialogueTypeTagIndex != -1)
                {
                    return dialogueTypeTags[dialogueTypeTagIndex].dialogueType;
                }
            }

            return fallbackValue;
        }

        public bool ContainsIgnoreTag(IList<string> tags)
        {
            return ContainsTag(tags, ignoreTag);
        }

        public bool ContainsStartTag(IList<string> tags)
        {
            return ContainsTag(tags, startTag);
        }

        public bool ContainsFinishTag(IList<string> tags)
        {
            return ContainsTag(tags, finishTag);
        }

        public bool ContainsDialogueTag(IList<string> tags)
        {
            return ContainsTag(tags, dialogueTag);
        }

        public bool ContainsChoiceTag(IList<string> tags)
        {
            return ContainsTag(tags, choiceTag);
        }

        public bool ContainsScriptTag(IList<string> tags)
        {
            return ContainsTag(tags, scriptTag);
        }

        public bool ContainsBranchTag(IList<string> tags)
        {
            return ContainsTag(tags, branchTag);
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

            if (string.CompareOrdinal(tag, finishTag) == 0)
            {
                return true;
            }

            if (string.CompareOrdinal(tag, dialogueTag) == 0)
            {
                return true;
            }

            if (string.CompareOrdinal(tag, choiceTag) == 0)
            {
                return true;
            }

            if (string.CompareOrdinal(tag, scriptTag) == 0)
            {
                return true;
            }

            if (string.CompareOrdinal(tag, branchTag) == 0)
            {
                return true;
            }

            if (uiPositionTags.Exists(x => string.CompareOrdinal(x.tag, tag) == 0))
            {
                return true;
            }

            if (dialogueTypeTags.Exists(x => string.CompareOrdinal(x.tag, tag) == 0))
            {
                return true;
            }

            if (IsRegisteredCharacterTag(tag))
            {
                return true;
            }

            return false;
        }

        public bool IsRegisteredCharacterTag(string tag)
        {
            return characterTags.Exists(x => string.CompareOrdinal(x.tag, tag) == 0);
        }

        #endregion

        #region Script Instruction Utility

        public bool IsSetParameterInstruction(string text)
        {
            return string.CompareOrdinal(text, setParameterInstruction) == 0;
        }

        public bool IsSetBackgroundInstruction(string text)
        {
            return string.CompareOrdinal(text, setBackgroundInstruction) == 0;
        }

        public bool IsSubNarrativeInstruction(string text)
        {
            return string.CompareOrdinal(text, subNarrativeInstruction) == 0;
        }

        public bool IsAddInventoryItemInstruction(string text)
        {
            return string.CompareOrdinal(text, addInventoryItemInstruction) == 0;
        }

        #endregion

        #region Branch Utility

        public bool IsRandomBranch(string text)
        {
            return string.CompareOrdinal(text, randomBranch) == 0;
        }

        #endregion

        #region Loca Tokens Utility

        public bool IsRegisteredLocaKey(string key)
        {
            return locaTokenKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public ScriptableObject FindLocaToken(string key)
        {
            return locaTokenKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).token;
        }

        public void FindLocaTokens(string text, TwineStoryAnalysis analysis)
        {
            foreach (string key in Tokens.Get(text, locaTokenStartDelimiter, locaTokenEndDelimiter))
            {
                if (IsRegisteredLocaKey(key))
                {
                    analysis.foundLocaTokens.Add(key);
                }
                else
                {
                    analysis.unrecognizedKeys.Add(key);
                }
            }
        }

        public string ReplaceLocaTokens(string dialogueText, List<ScriptableObject> locaTokens)
        {
            locaTokens.Clear();

            int currentToken = 0;
            int startDelimiterIndex = dialogueText.IndexOf(locaTokenStartDelimiter);

            while (startDelimiterIndex != -1)
            {
                int endDelimiterIndex = dialogueText.IndexOf(locaTokenEndDelimiter, startDelimiterIndex + 1);
                string key = dialogueText.Substring(startDelimiterIndex + 1, endDelimiterIndex - startDelimiterIndex - 1);
                string token = currentToken.ToString();
                dialogueText = dialogueText.Remove(startDelimiterIndex + 1, key.Length);
                dialogueText = dialogueText.Insert(startDelimiterIndex + 1, token);

                ScriptableObject scriptableObject = FindLocaToken(key);
                UnityEngine.Debug.Assert(scriptableObject != null, $"Could not find loca token for key {key} in node.");
                locaTokens.Add(scriptableObject);
                ++currentToken;

                startDelimiterIndex = dialogueText.IndexOf(locaTokenStartDelimiter, startDelimiterIndex + token.Length + 1);
            }

            return dialogueText.Trim();
        }

        #endregion

        #region Conditions Utility

        public bool IsRegisteredConditionKey(string key)
        {
            return conditionKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public Condition FindCondition(string key)
        {
            return conditionKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).condition;
        }

        public void FindConditions(string conditionText, TwineStoryAnalysis analysis)
        {
            foreach (string key in Tokens.Get(conditionText, conditionStartDelimiter, conditionEndDelimiter))
            {
                if (IsRegisteredConditionKey(key))
                {
                    analysis.foundConditions.Add(key);
                }
                else
                {
                    analysis.unrecognizedKeys.Add(key);
                }
            }
        }

        public string ReplaceConditions(string choiceDisplayText, List<Condition> conditions)
        {
            conditions.Clear();

            int currentToken = 0;
            int startDelimiterIndex = choiceDisplayText.IndexOf(conditionStartDelimiter);

            while (startDelimiterIndex != -1)
            {
                int endDelimiterIndex = choiceDisplayText.IndexOf(conditionEndDelimiter, startDelimiterIndex + 1);
                string key = choiceDisplayText.Substring(startDelimiterIndex + 1, endDelimiterIndex - startDelimiterIndex - 1);
                choiceDisplayText = choiceDisplayText.Remove(startDelimiterIndex, key.Length + 2); // Remove the delimiter markers too

                Condition condition = FindCondition(key);
                UnityEngine.Debug.Assert(condition != null, $"Could not find condition for key {key} in node.");
                conditions.Add(condition);
                ++currentToken;

                // We remove the whole condition string so continue where we left off minus 1 (because of removed leading delimiter)
                startDelimiterIndex = choiceDisplayText.IndexOf(conditionStartDelimiter, startDelimiterIndex - 1);
            }

            return choiceDisplayText.Trim();
        }

        #endregion

        #region Parameter Utility

        private string StripParameterDelimiters(string key)
        {
            if (!string.IsNullOrEmpty(key) &&
                key[0] == parameterStartDelimiter && 
                key[key.Length - 1] == parameterEndDelimiter)
            {
                key = key.Substring(1, key.Length - 2);
            }

            return key;
        }

        public bool IsRegisteredParameterKey(string key)
        {
            key = StripParameterDelimiters(key);
            return parameterKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public ScriptableObject FindParameter(string key)
        {
            key = StripParameterDelimiters(key);
            return parameterKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).parameter;
        }

        public void FindParameters(string text, TwineStoryAnalysis analysis)
        {
            foreach (string key in Tokens.Get(text, parameterStartDelimiter, parameterEndDelimiter))
            {
                if (IsRegisteredParameterKey(key))
                {
                    analysis.foundParameters.Add(key);
                }
                else
                {
                    analysis.unrecognizedKeys.Add(key);
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                string[] splitText = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (splitText != null &&
                    splitText.Length >= 2 &&
                    IsSetParameterInstruction(splitText[0]))
                {
                    string parameterName = splitText[1];

                    if (IsRegisteredParameterKey(parameterName))
                    {
                        analysis.foundParameters.Add(parameterName);
                    }
                    else
                    {
                        analysis.unrecognizedKeys.Add(parameterName);
                    }
                }
            }
        }

        #endregion

        #region Background Utility

        public bool IsRegisteredBackgroundKey(string key)
        {
            return backgroundKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public Background FindBackground(string key)
        {
            return backgroundKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).background;
        }

        public void FindBackgrounds(string nodeText, TwineStoryAnalysis analysis)
        {
            if (!string.IsNullOrWhiteSpace(nodeText))
            {
                string[] splitText = nodeText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (splitText != null && 
                    splitText.Length >= 2 &&
                    IsSetBackgroundInstruction(splitText[0]))
                {
                    string backgroundName = splitText[1];

                    if (IsRegisteredBackgroundKey(backgroundName))
                    {
                        analysis.foundBackgrounds.Add(backgroundName);
                    }
                    else
                    {
                        analysis.unrecognizedKeys.Add(backgroundName);
                    }
                }
            }
        }

        #endregion

        #region Sub Narrative Utility

        public bool IsRegisteredSubNarrativeKey(string key)
        {
            return subNarrativeKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public NarrativeGraph FindSubNarrative(string key)
        {
            return subNarrativeKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).subNarrative;
        }

        public void FindSubNarratives(string nodeText, TwineStoryAnalysis analysis)
        {
            if (!string.IsNullOrWhiteSpace(nodeText))
            {
                string[] splitText = nodeText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (splitText != null &&
                    splitText.Length >= 2 &&
                    IsSubNarrativeInstruction(splitText[0]))
                {
                    string subNarrativeName = splitText[1];

                    if (IsRegisteredSubNarrativeKey(subNarrativeName))
                    {
                        analysis.foundSubNarratives.Add(subNarrativeName);
                    }
                    else
                    {
                        analysis.unrecognizedKeys.Add(subNarrativeName);
                    }
                }
            }
        }

        #endregion

        #region Inventory Item Utility

        public bool IsRegisteredInventoryItemKey(string key)
        {
            return inventoryItemKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public InventoryItem FindInventoryItem(string key)
        {
            return inventoryItemKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).inventoryItem;
        }

        public void FindInventoryItems(string nodeText, TwineStoryAnalysis analysis)
        {
            if (!string.IsNullOrWhiteSpace(nodeText))
            {
                string[] splitText = nodeText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (splitText != null &&
                    splitText.Length >= 2 &&
                    IsAddInventoryItemInstruction(splitText[0]))
                {
                    string inventoryItemName = splitText[1];

                    if (IsRegisteredInventoryItemKey(inventoryItemName))
                    {
                        analysis.foundInventoryItems.Add(inventoryItemName);
                    }
                    else
                    {
                        analysis.unrecognizedKeys.Add(inventoryItemName);
                    }
                }
            }
        }

        #endregion
    }
}