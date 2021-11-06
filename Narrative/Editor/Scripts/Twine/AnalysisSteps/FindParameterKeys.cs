using CelesteEditor.Narrative.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindParameterKeys), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Parameter Keys")]
    public class FindParameterKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            return !string.IsNullOrWhiteSpace(twineNode.text);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            settings.FindParameters(twineNode.text, analysis);
        }
    }
}