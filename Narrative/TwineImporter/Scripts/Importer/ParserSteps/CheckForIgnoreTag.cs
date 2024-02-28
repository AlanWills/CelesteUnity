using Celeste.Twine;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = "CheckForIgnoreTag", order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Check For Ignore Tag")]
    public class CheckForIgnoreTag : TwineNodeParserStep
    {
        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            var tags = parseContext.TwineNode.Tags;
            return parseContext.ImporterSettings.ContainsIgnoreTag(tags);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            parseContext.StopAnalysing = true;
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return HasIgnoreTag(parseContext.ImporterSettings, parseContext.TwineNode);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.StopParsing = true;
        }

        #endregion

        private bool HasIgnoreTag(TwineStoryImporterSettings importerSettings, TwineNode twineNode)
        {
            return importerSettings.ContainsIgnoreTag(twineNode.Tags);
        }
    }
}