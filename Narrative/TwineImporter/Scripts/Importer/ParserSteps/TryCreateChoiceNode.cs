﻿using Celeste.DataStructures;
using Celeste.Twine;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateChoiceNode), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Create Choice Node")]
    public class TryCreateChoiceNode : TwineNodeParserStep, IUsesTags
    {
        #region Properties and Fields

        public IEnumerable<string> Tags
        {
            get { yield return choiceTag; }
        }

        [SerializeField] private string choiceTag = "Choice";

        #endregion

        public bool UsesTag(string tag)
        {
            return string.CompareOrdinal(tag, choiceTag) == 0;
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

            // We must have more than one link
            return twineNode.Links.Count > 1;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.FSMNode = parseContext.Graph.AddNode<ChoiceNode>();
        }

        #endregion

        private bool HasInstruction(IReadOnlyList<string> tags)
        {
            return tags.Contains(choiceTag);
        }
    }
}