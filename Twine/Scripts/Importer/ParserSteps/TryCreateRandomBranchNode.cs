using Celeste.FSM.Nodes.Logic;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateRandomBranchNode), menuName = "Celeste/Twine/Parser Steps/Try Create Random Branch Node")]
    public class TryCreateRandomBranchNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string nonLinkText = parseContext.StrippedLinksText;
            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (splitText == null || splitText.Length != 1)
            {
                return false;
            }

            return importerSettings.IsRandomBranch(splitText[0]);
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