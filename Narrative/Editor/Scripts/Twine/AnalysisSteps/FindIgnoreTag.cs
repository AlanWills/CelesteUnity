using Celeste.FSM.Nodes.Events;
using CelesteEditor.Narrative.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindIgnoreTag), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Ignore Tag")]
    public class FindIgnoreTag : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            var tags = parseContext.TwineNode.tags;
            return tags != null && parseContext.ImporterSettings.ContainsIgnoreTag(tags);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            parseContext.StopAnalysing = true;
        }
    }
}