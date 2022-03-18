using Celeste.Twine;
using Celeste.Narrative.TwineImporter.AnalysisSteps;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindConditionKeys), menuName = "Celeste/Twine/Analysis Steps/Find Condition Keys")]
    public class FindConditionKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            return twineNode.Links.Count > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            foreach (TwineNodeLink link in twineNode.Links)
            {
                settings.FindConditions(link.name, analysis);
            }
        }
    }
}