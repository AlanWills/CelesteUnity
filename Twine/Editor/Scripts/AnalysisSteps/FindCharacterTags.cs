using CelesteEditor.Twine.AnalysisSteps;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindCharacterTags), menuName = "Celeste/Twine/Analysis Steps/Find Character Tags")]
    public class FindCharacterTags : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            var tags = parseContext.TwineNode.Tags;
            return tags.Count > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            List<string> tags = parseContext.TwineNode.Tags;
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