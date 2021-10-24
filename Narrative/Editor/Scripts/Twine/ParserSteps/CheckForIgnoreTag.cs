using Celeste.FSM;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "CheckForIgnoreTag", menuName = "Celeste/Narrative/Twine/Parser Steps/Check For Ignore Tag")]
    public class CheckForIgnoreTag : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.ImporterSettings.ContainsIgnoreTag(parseContext.TwineNode.tags);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.StopParsing = true;
        }
    }
}