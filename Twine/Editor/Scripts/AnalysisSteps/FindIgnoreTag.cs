using CelesteEditor.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindIgnoreTag), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Ignore Tag")]
    public class FindIgnoreTag : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            var tags = parseContext.TwineNode.tags;
            return parseContext.ImporterSettings.ContainsIgnoreTag(tags);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            parseContext.StopAnalysing = true;
        }
    }
}