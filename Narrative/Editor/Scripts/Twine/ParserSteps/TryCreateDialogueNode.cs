using Celeste.FSM;
using Celeste.Narrative;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateDialogueNode", menuName = "Celeste/Narrative/Twine/Parser Steps/Try Create Dialogue Node")]
    public class TryCreateDialogueNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            if (parseContext.TwineNode.links == null)
            {
                return false;
            }

            return parseContext.TwineNode.links.Length == 1;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.FSMNode = parseContext.Graph.AddNode<DialogueNode>();
        }
    }
}