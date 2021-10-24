using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using CelesteEditor.Narrative.Nodes;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddNodeInfo", menuName = "Celeste/Narrative/Twine/Parser Steps/Try Add Node Info")]
    public class TryAddNodeInfo : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode != null;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            TwineNode node = parseContext.TwineNode;
            FSMNode fsmNode = parseContext.FSMNode;

            fsmNode.name = node.name;
            fsmNode.position = (node.position - parseContext.StartingNodePosition) * importerSettings.nodeSpread;
        }
    }
}