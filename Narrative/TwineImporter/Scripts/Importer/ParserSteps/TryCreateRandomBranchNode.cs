using Celeste.FSM.Nodes.Logic;
using Celeste.Parameters;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateRandomBranchNode), menuName = "Celeste/Twine/Parser Steps/Try Create Random Branch Node")]
    public class TryCreateRandomBranchNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private StringValue instruction;

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            string[] splitText = parseContext.SplitStrippedLinksText;

            if (splitText == null || splitText.Length != 1)
            {
                return false;
            }

            return string.CompareOrdinal(splitText[0], instruction.Value) == 0;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            RandomBranchNode randomBranchNode = parseContext.Graph.AddNode<RandomBranchNode>();
            parseContext.FSMNode = randomBranchNode;

            foreach (TwineNodeLink link in parseContext.TwineNode.Links)
            {
                randomBranchNode.AddOutputPort(link.name);
            }
        }
    }
}