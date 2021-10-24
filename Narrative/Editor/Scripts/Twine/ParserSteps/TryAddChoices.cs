using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using CelesteEditor.Narrative.Nodes;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddChoices", menuName = "Celeste/Narrative/Twine/Parser Steps/Try Add Choices")]
    public class TryAddChoices : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is IChoiceNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            TwineNode node = parseContext.TwineNode;
            IChoiceNode choiceNode = parseContext.FSMNode as IChoiceNode;
            ChoiceNodeBuilder choiceNodeBuilder = ChoiceNodeBuilder.WithNode(choiceNode);

            foreach (TwineNodeLink link in node.links)
            {
                choiceNodeBuilder.WithTextChoice(link.link, link.name);
            }
        }
    }
}