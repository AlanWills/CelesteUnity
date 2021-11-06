using Celeste.FSM.Nodes.Events;
using CelesteEditor.Narrative.Twine.AnalysisSteps;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(FindLocaTokenKeys), menuName = "Celeste/Narrative/Twine/Analysis Steps/Find Loca Token Keys")]
    public class FindLocaTokenKeys : TwineNodeAnalysisStep
    {
        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            bool hasText = !string.IsNullOrWhiteSpace(twineNode.text);
            bool hasLinks = twineNode.links != null && twineNode.links.Length > 0;

            return hasText || hasLinks;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings settings = parseContext.ImporterSettings;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            // Find Loca Tokens in node text
            {
                settings.FindLocaTokens(twineNode.text, analysis);
            }

            // Find Loca Tokens in link display text
            {
                var links = twineNode.links;

                foreach (TwineNodeLink link in links)
                {
                    settings.FindLocaTokens(link.name, analysis);
                }
            }
        }
    }
}