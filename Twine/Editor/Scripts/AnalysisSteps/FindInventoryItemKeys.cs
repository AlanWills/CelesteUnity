using Celeste.Twine;
using CelesteEditor.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindInventoryItemKeys), menuName = "Celeste/Twine/Analysis Steps/Find Inventory Item Keys")]
    public class FindInventoryItemKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return !string.IsNullOrWhiteSpace(parseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;
            settings.FindInventoryItems(parseContext.StrippedLinksText, analysis);
        }
    }
}