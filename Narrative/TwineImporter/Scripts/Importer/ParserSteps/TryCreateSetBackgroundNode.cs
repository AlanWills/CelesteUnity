using Celeste.Events;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Nodes.Events;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateSetBackgroundNode", menuName = "Celeste/Twine/Parser Steps/Try Create Set Background Node")]
    public class TryCreateSetBackgroundNode : TwineNodeParserStep, IUsesKeys
    {
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

        #region Properties and Fields

        [SerializeField] private string instruction = "SetBackground";
        [SerializeField] private BackgroundEvent setBackgroundEvent;
        [SerializeField] private List<BackgroundKey> backgroundKeys = new List<BackgroundKey>();

        #endregion

        public void AddKeyForUse(string key, object background)
        {
            backgroundKeys.Add(new BackgroundKey(key, background as Background));
        }

        public bool CouldUseKey(string key, object background)
        {
            return background is Background;
        }

        public bool UsesKey(string key)
        {
            return backgroundKeys.Exists((x) => string.CompareOrdinal(x.key, key) == 0);
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext analyseContext)
        {
            return !string.IsNullOrWhiteSpace(analyseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext analyseContext)
        {
            FindBackgrounds(analyseContext.SplitStrippedLinksText, analyseContext.Analysis);
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            string[] splitText = parseContext.SplitStrippedLinksText;

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (!IsInstruction(splitText[0]))
            {
                return false;
            }

            return HasBackground(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            string[] splitText = parseContext.SplitStrippedLinksText;

            Background background = FindBackground(splitText[1]);
            BackgroundEventRaiserNode backgroundEventRaiserNode = parseContext.Graph.AddNode<BackgroundEventRaiserNode>();
            backgroundEventRaiserNode.argument.Value = background;
            backgroundEventRaiserNode.toRaise = setBackgroundEvent;

            parseContext.FSMNode = backgroundEventRaiserNode;
        }

        #endregion

        private bool IsInstruction(string str)
        {
            return string.CompareOrdinal(instruction, str) == 0;
        }

        private bool HasBackground(string key)
        {
            return backgroundKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        private Background FindBackground(string key)
        {
            return backgroundKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).background;
        }

        private void FindBackgrounds(string[] splitStrippedLinkText, TwineStoryAnalysis analysis)
        {
            if (splitStrippedLinkText != null &&
                splitStrippedLinkText.Length >= 2 &&
                IsInstruction(splitStrippedLinkText[0]))
            {
                string backgroundName = splitStrippedLinkText[1];

                if (HasBackground(backgroundName))
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
}