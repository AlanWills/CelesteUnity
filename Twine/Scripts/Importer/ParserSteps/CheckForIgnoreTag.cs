using Celeste.Twine;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "CheckForIgnoreTag", menuName = "Celeste/Twine/Parser Steps/Check For Ignore Tag")]
    public class CheckForIgnoreTag : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return HasIgnoreTag(parseContext.ImporterSettings, parseContext.TwineNode);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.StopParsing = true;
        }

        private bool HasIgnoreTag(TwineStoryImporterSettings importerSettings, TwineNode twineNode)
        {
            return importerSettings.ContainsIgnoreTag(twineNode.Tags);
        }
    }
}