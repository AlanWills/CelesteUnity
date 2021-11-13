using CelesteEditor.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindConditionKeys), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Condition Keys")]
    public class FindConditionKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            return twineNode.links != null && twineNode.links.Length > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            foreach (TwineNodeLink link in twineNode.links)
            {
                settings.FindConditions(link.name, analysis);
            }
        }
    }
}