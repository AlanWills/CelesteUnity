using Celeste.Narrative.TwineImporter.AnalysisSteps;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindIgnoreTag), menuName = "Celeste/Twine/Analysis Steps/Find Ignore Tag")]
    public class FindIgnoreTag : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            var tags = parseContext.TwineNode.Tags;
            return parseContext.ImporterSettings.ContainsIgnoreTag(tags);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            parseContext.StopAnalysing = true;
        }
    }
}