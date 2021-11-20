using CelesteEditor.Twine.AnalysisSteps;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindUnrecognizedTags), menuName = "Celeste/Twine/Analysis Steps/Find Unrecognized Tags")]
    public class FindUnrecognizedTags : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            var tags = parseContext.TwineNode.tags;
            return tags.Count > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            List<string> tags = parseContext.TwineNode.tags;
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