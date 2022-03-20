using Celeste.FSM.Nodes.Events;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateFinishNode), menuName = "Celeste/Twine/Parser Steps/Try Create Finish Node")]
    public class TryCreateFinishNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private Celeste.Events.Event finishEvent;

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return HasFinishTag(parseContext.ImporterSettings, parseContext.TwineNode);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            EventRaiserNode eventNode = parseContext.Graph.AddNode<EventRaiserNode>();
            eventNode.toRaise = finishEvent;
            parseContext.FSMNode = eventNode;
            parseContext.Graph.finishNode = eventNode;
        }

        private bool HasFinishTag(TwineStoryImporterSettings importerSettings, TwineNode twineNode)
        {
            return importerSettings.ContainsFinishTag(twineNode.Tags);
        }
    }
}