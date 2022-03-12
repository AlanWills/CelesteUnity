using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Twine;
using System.Collections;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateScriptNode), menuName = "Celeste/Twine/Parser Steps/Try Create Dialogue Node")]
    public class TryCreateDialogueNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            if (importerSettings.ContainsDialogueTag(twineNode.Tags))
            {
                // If we have the dialogue tag, we parse this as dialogue no matter what
                return true;
            }

            // We must have only one link and also a valid character set
            return twineNode.Links.Count == 1 &&
                   importerSettings.FindCharacterInTags(twineNode.Tags) != null;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            parseContext.FSMNode = parseContext.Graph.AddNode<DialogueNode>();
        }
    }
}