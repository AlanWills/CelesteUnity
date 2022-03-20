using Celeste.Events;
using Celeste.FSM.Nodes.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreatePlaySFXNode), menuName = "Celeste/Twine/Parser Steps/Try Create Play SFX Node")]
    public class TryCreatePlaySFXNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private StringValue instruction;
        [SerializeField] private AudioClipEvent playSFXEvent;

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

            if (string.CompareOrdinal(instruction.Value, splitText[0]) != 0)
            {
                return false;
            }

            return importerSettings.IsRegisteredAudioClipKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string[] splitText = parseContext.SplitStrippedLinksText;

            AudioClip audioClip = importerSettings.FindAudioClip(splitText[1]);
            AudioClipEventRaiserNode audioClipEventRaiserNode = parseContext.Graph.AddNode<AudioClipEventRaiserNode>();
            audioClipEventRaiserNode.argument.Value = audioClip;
            audioClipEventRaiserNode.toRaise = playSFXEvent;

            parseContext.FSMNode = audioClipEventRaiserNode;
        }
    }
}