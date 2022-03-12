using Celeste.FSM;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    public class TwineNodeParseContext
    {
        private TwineNode twineNode = default;
        public TwineNode TwineNode
        {
            get { return twineNode; }
            set
            {
                twineNode = value;
                StrippedLinksText = twineNode != null ? ImporterSettings.StripLinksFromText(twineNode.Text) : string.Empty;
            }
        }

        public string StrippedLinksText { get; private set; }
        public FSMGraph Graph { get; set; }
        public TwineStoryImporterSettings ImporterSettings { get; set; }
        public Vector2 StartingNodePosition { get; set; }
        public bool StopParsing { get; set; }
        public FSMNode FSMNode { get; set; }
    }

    public abstract class TwineNodeParserStep : ScriptableObject
    {
        public abstract bool CanParse(TwineNodeParseContext parseContext);
        public abstract void Parse(TwineNodeParseContext parseContext);
    }
}