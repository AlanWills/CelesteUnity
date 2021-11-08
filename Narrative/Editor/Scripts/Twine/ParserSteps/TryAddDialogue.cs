using Celeste.Narrative;
using CelesteEditor.Narrative.Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddDialogue", menuName = "Celeste/Narrative/Twine/Parser Steps/Try Add Dialogue")]
    public class TryAddDialogue : TwineNodeParserStep
    {
        #region Properties and Fields

        [NonSerialized] private List<ScriptableObject> locaTokens = new List<ScriptableObject>();

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is IDialogueNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            TwineNode node = parseContext.TwineNode;
            IDialogueNode dialogueNode = parseContext.FSMNode as IDialogueNode;

            string dialogueText = node.text;
            dialogueText = importerSettings.StripLinksFromText(dialogueText);
            dialogueText = importerSettings.ReplaceLocaTokens(dialogueText, locaTokens);

            DialogueNodeBuilder.
                        WithNode(dialogueNode).
                        WithRawDialogue(dialogueText).
                        WithUIPosition(importerSettings.FindUIPositionFromTag(node.tags, dialogueNode.UIPosition)).
                        WithDialogueType(importerSettings.FindDialogueTypeFromTag(node.tags, DialogueType.Speech)).
                        WithDialogueTokens(locaTokens.ToArray());
        }
    }
}