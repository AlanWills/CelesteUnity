using Celeste.FSM;
using Celeste.Narrative;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateDialogueNode", menuName = "Celeste/Twine/Parser Steps/Try Create Dialogue Node")]
    public class TryCreateDialogueNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            return parseContext.TwineNode.links.Count == 1;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.FSMNode = parseContext.Graph.AddNode<DialogueNode>();
        }
    }
}