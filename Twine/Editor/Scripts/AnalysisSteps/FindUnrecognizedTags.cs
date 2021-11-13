using CelesteEditor.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindUnrecognizedTags), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Unrecognized Tags")]
    public class FindUnrecognizedTags : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            var tags = parseContext.TwineNode.tags;
            return tags != null && tags.Length > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            string[] tags = parseContext.TwineNode.tags;
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            foreach (string tag in tags)
            {
                if (!settings.IsRecognizedTag(tag))
                {
                    analysis.unrecognizedTags.Add(tag);
                }
            }
        }
    }
}