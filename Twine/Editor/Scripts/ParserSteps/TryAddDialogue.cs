using Celeste.Narrative;
using Celeste.Narrative.Nodes;
using Celeste.Twine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddDialogue", menuName = "Celeste/Twine/Parser Steps/Try Add Dialogue")]
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

            string dialogueText = node.Text;
            dialogueText = importerSettings.StripLinksFromText(dialogueText);
            dialogueText = importerSettings.ReplaceLocaTokens(dialogueText, locaTokens);

            DialogueNodeBuilder.
                        WithNode(dialogueNode).
                        WithRawDialogue(dialogueText).
                        WithUIPosition(importerSettings.FindUIPositionFromTag(node.Tags, dialogueNode.UIPosition)).
                        WithDialogueType(importerSettings.FindDialogueTypeFromTag(node.Tags, DialogueType.Speech)).
                        WithDialogueTokens(locaTokens.ToArray());
        }
    }
}