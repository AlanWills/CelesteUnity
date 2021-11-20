using Celeste.FSM;
using Celeste.Narrative;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateChoiceNode", menuName = "Celeste/Twine/Parser Steps/Try Create Choice Node")]
    public class TryCreateChoiceNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            return parseContext.TwineNode.links.Count > 1;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.FSMNode = parseContext.Graph.AddNode<ChoiceNode>();
        }
    }
}