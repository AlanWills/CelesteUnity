using Celeste.FSM.Nodes.Logic;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateRandomBranchNode), menuName = "Celeste/Twine/Parser Steps/Try Create Random Branch Node")]
    public class TryCreateRandomBranchNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private string instruction = "Random";

        #endregion

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext analyseContext)
        {
            return false;
        }

        public override void Analyse(TwineNodeAnalyseContext analyseContext)
        {
        }

        #endregion

        #region Parse

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

            return string.CompareOrdinal(splitText[0], instruction) == 0;
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

        #endregion
    }
}