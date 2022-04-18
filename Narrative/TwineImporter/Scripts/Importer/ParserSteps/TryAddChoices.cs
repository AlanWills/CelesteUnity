using Celeste.Logic;
using Celeste.Narrative.Nodes;
using Celeste.Narrative.Tokens;
using Celeste.Twine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    #region Condition Key Struct

    [Serializable]
    public struct ConditionKey : IKey
    {
        string IKey.Key => key;

        public string key;
        public Condition condition;

        public ConditionKey(string key, Condition condition)
        {
            this.key = key;
            this.condition = condition;
        }
    }

    #endregion

    [CreateAssetMenu(fileName = "TryAddChoices", menuName = "Celeste/Twine/Parser Steps/Try Add Choices")]
    public class TryAddChoices : TwineNodeParserStep, IUsesKeys
    {
        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var condition in conditionKeys)
                {
                    yield return condition.key;
                }
            }
        }

        [SerializeField] private LocaTokens locaTokens;
        [SerializeField] private char conditionStartDelimiter = '#';
        [SerializeField] private char conditionEndDelimiter = '#';
        [SerializeField] private List<ConditionKey> conditionKeys = new List<ConditionKey>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            conditionKeys.Add((ConditionKey)key);
        }

        public bool CouldUseKey(IKey key)
        {
            return key is ConditionKey;
        }

        public bool UsesKey(IKey key)
        {
            return conditionKeys.Exists(x => string.CompareOrdinal(x.key, key.Key) == 0);
        }
        
        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext analyseContext)
        {
            return analyseContext.TwineNode.Links.Count > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext analyseContext)
        {
            FindConditions(analyseContext);
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is IChoiceNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineNode node = parseContext.TwineNode;
            IChoiceNode choiceNode = parseContext.FSMNode as IChoiceNode;
            ChoiceNodeBuilder choiceNodeBuilder = ChoiceNodeBuilder.WithNode(choiceNode);

            List<UnityEngine.Object> foundLocaTokens = new List<UnityEngine.Object>();
            List<Condition> foundConditions = new List<Condition>();

            foreach (TwineNodeLink link in node.Links)
            {
                string choiceDisplayText = ReplaceConditions(link.name, foundConditions);

                if (locaTokens != null)
                {
                    choiceDisplayText = locaTokens.ReplaceLocaTokens(choiceDisplayText, foundLocaTokens);
                }

                choiceNodeBuilder.WithTextChoice(
                    link.link,
                    choiceDisplayText,
                    foundLocaTokens.ToArray(),
                    foundConditions.ToArray());
            }
        }

        #endregion

        #region Conditions Utility

        private bool HasCondition(string key)
        {
            return conditionKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        private Condition FindCondition(string key)
        {
            return conditionKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).condition;
        }

        private void FindConditions(TwineNodeAnalyseContext analyseContext)
        {
            TwineNode twineNode = analyseContext.TwineNode;
            TwineStoryAnalysis analysis = analyseContext.Analysis;

            foreach (TwineNodeLink link in twineNode.Links)
            {
                foreach (string key in Twine.Tokens.Get(link.name, conditionStartDelimiter, conditionEndDelimiter))
                {
                    if (HasCondition(key))
                    {
                        analysis.AddFoundConditions(key);
                    }
                    else
                    {
                        analysis.AddUnrecognizedKey(key);
                    }
                }
            }
        }

        private string ReplaceConditions(string choiceDisplayText, List<Condition> conditions)
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
    }
}