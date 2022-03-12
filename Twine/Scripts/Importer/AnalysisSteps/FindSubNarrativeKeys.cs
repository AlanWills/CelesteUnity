using Celeste.Twine;
using Celeste.Twine.AnalysisSteps;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindSubNarrativeKeys), menuName = "Celeste/Twine/Analysis Steps/Find Sub Narrative Keys")]
    public class FindSubNarrativeKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return !string.IsNullOrWhiteSpace(parseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            settings.FindSubNarratives(parseContext.StrippedLinksText, analysis);
        }
    }
}