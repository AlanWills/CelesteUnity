using Celeste.Twine;
using CelesteEditor.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindBackgroundKeys), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Background Keys")]
    public class FindBackgroundKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            string nonLinkText = parseContext.ImporterSettings.StripLinksFromText(twineNode.text);
            return !string.IsNullOrWhiteSpace(nonLinkText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            string nonLinkText = parseContext.ImporterSettings.StripLinksFromText(twineNode.text);
            settings.FindBackgrounds(nonLinkText, analysis);
        }
    }
}