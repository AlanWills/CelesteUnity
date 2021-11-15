using Celeste.FSM;
using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
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
            fsmNode.position = (node.Position - parseContext.StartingNodePosition) * importerSettings.nodeSpread;
        }
    }
}