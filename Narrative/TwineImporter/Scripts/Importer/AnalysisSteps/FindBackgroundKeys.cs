using Celeste.Narrative.TwineImporter.AnalysisSteps;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindBackgroundKeys), menuName = "Celeste/Twine/Analysis Steps/Find Background Keys")]
    public class FindBackgroundKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return !string.IsNullOrWhiteSpace(parseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            settings.FindBackgrounds(parseContext.StrippedLinksText, analysis);
        }
    }
}