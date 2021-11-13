using Celeste.FSM.Nodes.Events;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "CheckForFinishTag", menuName = "Celeste/Narrative/Twine/Parser Steps/Check For Finish Tag")]
    public class CheckForFinishTag : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return HasFinishTag(parseContext.ImporterSettings, parseContext.TwineNode);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            EventRaiserNode eventNode = parseContext.Graph.AddNode<EventRaiserNode>();
            eventNode.toRaise = parseContext.ImporterSettings.finishEvent;
            parseContext.FSMNode = eventNode;
            parseContext.Graph.finishNode = eventNode;
        }

        private bool HasFinishTag(TwineStoryImporterSettings importerSettings, TwineNode twineNode)
        {
            return importerSettings.ContainsFinishTag(twineNode.tags);
        }
    }
}