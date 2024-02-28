﻿using Celeste.DataStructures;
using Celeste.Twine;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateScriptNode), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Create Dialogue Node")]
    public class TryCreateDialogueNode : TwineNodeParserStep, IUsesTags
    {
        #region Properties and Fields

        public IEnumerable<string> Tags
        {
            get { yield return dialogueTag; }
        }

        [SerializeField] private string dialogueTag = "Dialogue";

        #endregion

        public bool UsesTag(string tag)
        {
            return string.CompareOrdinal(tag, dialogueTag) == 0;
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext analyseContext)
        {
            return false;
        }

        public override void Analyse(TwineNodeAnalyseContext analyseContext)
        {
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineNode twineNode = parseContext.TwineNode;

            if (HasInstruction(twineNode.Tags))
            {
                // If we have the dialogue tag, we parse this as dialogue no matter what
                return true;
            }

            // We must have at most one link - no branching allowed
            return twineNode.Links.Count <= 1;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.FSMNode = parseContext.Graph.AddNode<DialogueNode>();
        }

        #endregion

        private bool HasInstruction(IReadOnlyList<string> tags)
        {
            return tags.Contains(dialogueTag);
        }
    }
}