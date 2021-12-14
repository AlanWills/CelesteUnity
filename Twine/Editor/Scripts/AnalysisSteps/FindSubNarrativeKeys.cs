using Celeste.Twine;
using CelesteEditor.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindSubNarrativeKeys), menuName = "Celeste/Twine/Analysis Steps/Find Sub Narrative Keys")]
    public class FindSubNarrativeKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            string nonLinkText = parseContext.ImporterSettings.StripLinksFromText(twineNode.Text);
            return !string.IsNullOrWhiteSpace(nonLinkText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            string nonLinkText = parseContext.ImporterSettings.StripLinksFromText(twineNode.Text);
            settings.FindSubNarratives(nonLinkText, analysis);
        }
    }
}