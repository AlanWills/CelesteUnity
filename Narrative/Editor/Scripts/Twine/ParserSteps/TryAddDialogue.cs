using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using CelesteEditor.Narrative.Nodes;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddDialogue", menuName = "Celeste/Narrative/Twine/Parser Steps/Try Add Dialogue")]
    public class TryAddDialogue : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is IDialogueNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            TwineNode node = parseContext.TwineNode;
            IDialogueNode dialogueNode = parseContext.FSMNode as IDialogueNode;

            Character character = importerSettings.FindCharacterFromTag(node.tags);
            Debug.Assert(character != null, $"Could not find character for node {node.name} ({node.pid}).");
            UIPosition characterDefaultPosition = character != null ? character.DefaultUIPosition : UIPosition.Centre;

            DialogueNodeBuilder.
                        WithNode(dialogueNode).
                        WithRawDialogue(node.text).
                        WithCharacter(character).
                        WithUIPosition(importerSettings.FindUIPositionFromTag(node.tags, characterDefaultPosition));
        }
    }
}