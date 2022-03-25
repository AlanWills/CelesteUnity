using Celeste.FSM.Nodes.Events;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateFinishNode), menuName = "Celeste/Twine/Parser Steps/Try Create Finish Node")]
    public class TryCreateFinishNode : TwineNodeParserStep, IUsesTags
    {
        #region Properties and Fields

        [SerializeField] private string finishTag = "Finish";
        [SerializeField] private Celeste.Events.Event finishEvent;

        #endregion

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
            return HasFinishTag(parseContext.TwineNode);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            EventRaiserNode eventNode = parseContext.Graph.AddNode<EventRaiserNode>();
            eventNode.toRaise = finishEvent;
            parseContext.FSMNode = eventNode;
            parseContext.Graph.finishNode = eventNode;
        }

        #endregion

        public bool UsesTag(string tag)
        {
            return string.CompareOrdinal(tag, finishTag) == 0;
        }

        private bool HasFinishTag(TwineNode twineNode)
        {
            return twineNode.Tags.Exists((string s) => string.CompareOrdinal(s, finishTag) == 0);
        }
    }
}