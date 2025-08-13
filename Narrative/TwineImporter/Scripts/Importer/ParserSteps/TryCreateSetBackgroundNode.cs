using Celeste.Events;
using Celeste.Narrative.Backgrounds;
using Celeste.Narrative.Nodes.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    #region Background Key Struct

    [Serializable]
    public struct BackgroundKey: IKey
    {
        string IKey.Key => key;

        public string key;
        public Background background;

        public BackgroundKey(string key, Background background)
        {
            this.key = key;
            this.background = background;
        }
    }

    #endregion

    [CreateAssetMenu(fileName = "TryCreateSetBackgroundNode", order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Create Set Background Node")]
    public class TryCreateSetBackgroundNode : TwineNodeParserStep, IUsesKeys
    {
        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var backgroundKey in backgroundKeys)
                {
                    yield return backgroundKey.key;
                }
            }
        }

        [SerializeField] private string instruction = "SetBackground";
        [SerializeField] private SetBackgroundEvent setBackgroundEvent;
        [SerializeField] private List<BackgroundKey> backgroundKeys = new List<BackgroundKey>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            backgroundKeys.Add((BackgroundKey)key);
        }

        public bool CouldUseKey(IKey key)
        {
            return key is BackgroundKey;
        }

        public bool UsesKey(IKey key)
        {
            return backgroundKeys.Exists((x) => string.CompareOrdinal(x.key, key.Key) == 0);
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
            SetBackgroundEventRaiserNode backgroundEventRaiserNode = parseContext.Graph.AddNode<SetBackgroundEventRaiserNode>();
            backgroundEventRaiserNode.argument.Background = background;
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
                    analysis.AddFoundBackground(backgroundName);
                }
                else
                {
                    analysis.AddUnrecognizedKey(backgroundName);
                }
            }
        }
    }
}