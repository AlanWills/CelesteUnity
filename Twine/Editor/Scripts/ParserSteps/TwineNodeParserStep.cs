using Celeste.FSM;
using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    public class TwineNodeParseContext
    {
        public TwineNode TwineNode { get; set; }
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