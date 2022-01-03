using Celeste.Narrative;
using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateChoiceNode), menuName = "Celeste/Twine/Parser Steps/Try Create Choice Node")]
    public class TryCreateChoiceNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            if (importerSettings.ContainsChoiceTag(twineNode.Tags))
            {
                // If we have the dialogue tag, we parse this as dialogue no matter what
                return true;
            }

            // We must have more than one link and also a valid character set
            return twineNode.Links.Count > 1 &&
                   importerSettings.FindCharacterInTags(twineNode.Tags) != null;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.FSMNode = parseContext.Graph.AddNode<ChoiceNode>();
        }
    }
}