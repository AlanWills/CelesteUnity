using Celeste.Narrative.Nodes;
using Celeste.Narrative.Tokens;
using Celeste.Narrative.UI;
using Celeste.Twine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddDialogue", order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Add Dialogue")]
    public class TryAddDialogue : TwineNodeParserStep, IUsesTags, IUsesKeys
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

        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                for (int i = 0, n = locaTokens.NumItems; i < n; i++)
                {
                    yield return locaTokens.GetItem(i).key;
                }
            }
        }

        public IEnumerable<string> Tags
        {
            get
            {
                foreach (var uiPosition in uiPositionTags)
                {
                    yield return uiPosition.tag;
                }

                foreach (var dialogueType in dialogueTypeTags)
                {
                    yield return dialogueType.tag;
                }
            }
        }

        [SerializeField] private LocaTokens locaTokens;
        [SerializeField] private List<UIPositionTag> uiPositionTags = new List<UIPositionTag>();
        [SerializeField] private List<DialogueTypeTag> dialogueTypeTags = new List<DialogueTypeTag>();

        #endregion

        public bool UsesTag(string tag)
        {
            if (uiPositionTags.Exists(x => string.CompareOrdinal(x.tag, tag) == 0))
            {
                return true;
            }

            if (dialogueTypeTags.Exists(x => string.CompareOrdinal(x.tag, tag) == 0))
            {
                return true;
            }

            return false;
        }

        public void AddKeyForUse(IKey key)
        {
            if (locaTokens != null)
            {
                locaTokens.AddItem((LocaToken)key);
            }
        }

        public bool CouldUseKey(IKey key)
        {
            return key is LocaToken;
        }

        public bool UsesKey(IKey key)
        {
            if (locaTokens != null && locaTokens.HasLocaToken(key.Key))
            {
                return true;
            }

            return false;
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            bool hasText = !string.IsNullOrWhiteSpace(twineNode.Text);
            bool hasLinks = twineNode.Links.Count > 0;

            return hasText || hasLinks;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            // Find Loca Tokens in node text
            {
                FindLocaTokens(twineNode.Text, analysis);
            }

            // Find Loca Tokens in link display text
            {
                var links = twineNode.Links;

                foreach (TwineNodeLink link in links)
                {
                    FindLocaTokens(link.name, analysis);
                }
            }
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is IDialogueNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineNode node = parseContext.TwineNode;
            IDialogueNode dialogueNode = parseContext.FSMNode as IDialogueNode;

            string dialogueText = parseContext.StrippedLinksText;
            List<LocaToken> foundLocaTokens = locaTokens != null ? locaTokens.FindLocaTokens(dialogueText) : new List<LocaToken>();

            DialogueNodeBuilder.
                        WithNode(dialogueNode).
                        WithRawDialogue(dialogueText).
                        WithUIPosition(FindUIPositionFromTag(node.Tags, dialogueNode.UIPosition)).
                        WithDialogueType(FindDialogueTypeFromTag(node.Tags, DialogueType.Speech)).
                        WithDialogueTokens(foundLocaTokens);
        }

        #endregion

        private UIPosition FindUIPositionFromTag(IReadOnlyList<string> tags, UIPosition fallbackValue)
        {
            for (int i = 0, n = tags.Count; i < n; ++i)
            {
                var positionTagIndex = uiPositionTags.FindIndex(x => string.CompareOrdinal(x.tag, tags[i]) == 0);
                if (positionTagIndex != -1)
                {
                    return uiPositionTags[positionTagIndex].position;
                }
            }

            return fallbackValue;
        }

        private DialogueType FindDialogueTypeFromTag(IReadOnlyList<string> tags, DialogueType fallbackValue)
        {
            for (int i = 0, n = tags.Count; i < n; ++i)
            {
                var dialogueTypeTagIndex = dialogueTypeTags.FindIndex(x => string.CompareOrdinal(x.tag, tags[i]) == 0);
                if (dialogueTypeTagIndex != -1)
                {
                    return dialogueTypeTags[dialogueTypeTagIndex].dialogueType;
                }
            }

            return fallbackValue;
        }

        public void FindLocaTokens(string text, TwineStoryAnalysis analysis)
        {
            if (locaTokens == null)
            {
                return;
            }

            foreach (string key in Twine.Tokens.Get(text, locaTokens.StartDelimiter, locaTokens.EndDelimiter))
            {
                if (locaTokens.HasLocaToken(key))
                {
                    analysis.AddFoundLocaToken(key);
                }
                else
                {
                    analysis.AddUnrecognizedKey(key);
                }
            }
        }
    }
}