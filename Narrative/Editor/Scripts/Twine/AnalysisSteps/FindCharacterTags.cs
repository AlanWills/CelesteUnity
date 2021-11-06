using Celeste.FSM.Nodes.Events;
using CelesteEditor.Narrative.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindCharacterTags), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Character Tags")]
    public class FindCharacterTags : TwineNodeAnalysisStep
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
                if (settings.IsRegisteredCharacterTag(tag))
                {
                    analysis.foundCharacters.Add(tag);
                }
            }
        }
    }
}