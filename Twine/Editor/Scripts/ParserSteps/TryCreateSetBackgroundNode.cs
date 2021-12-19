using Celeste.FSM.Nodes.Parameters;
using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Nodes.Events;
using Celeste.Parameters;
using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateSetBackgroundNode", menuName = "Celeste/Twine/Parser Steps/Try Create Set Background Node")]
    public class TryCreateSetBackgroundNode : TwineNodeParserStep
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

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (!importerSettings.IsSetBackgroundInstruction(splitText[0]))
            {
                return false;
            }

            return importerSettings.IsRegisteredBackgroundKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            string nonLinkText = parseContext.StrippedLinksText;
            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            Background background = importerSettings.FindBackground(splitText[1]);
            BackgroundEventRaiserNode backgroundEventRaiserNode = parseContext.Graph.AddNode<BackgroundEventRaiserNode>();
            backgroundEventRaiserNode.argument.Value = background;
            backgroundEventRaiserNode.toRaise = importerSettings.setBackgroundEvent;

            parseContext.FSMNode = backgroundEventRaiserNode;
        }
    }
}