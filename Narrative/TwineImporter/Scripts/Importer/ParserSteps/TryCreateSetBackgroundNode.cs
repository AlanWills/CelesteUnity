using Celeste.Events;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Nodes.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateSetBackgroundNode", menuName = "Celeste/Twine/Parser Steps/Try Create Set Background Node")]
    public class TryCreateSetBackgroundNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private StringValue instruction;
        [SerializeField] private BackgroundEvent setBackgroundEvent;

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string[] splitText = parseContext.SplitStrippedLinksText;

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (string.CompareOrdinal(splitText[0], instruction.Value) != 0)
            {
                return false;
            }

            return importerSettings.IsRegisteredBackgroundKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string[] splitText = parseContext.SplitStrippedLinksText;

            Background background = importerSettings.FindBackground(splitText[1]);
            BackgroundEventRaiserNode backgroundEventRaiserNode = parseContext.Graph.AddNode<BackgroundEventRaiserNode>();
            backgroundEventRaiserNode.argument.Value = background;
            backgroundEventRaiserNode.toRaise = setBackgroundEvent;

            parseContext.FSMNode = backgroundEventRaiserNode;
        }
    }
}