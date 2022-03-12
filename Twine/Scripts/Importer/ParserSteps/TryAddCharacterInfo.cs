using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using Celeste.Twine;
using Celeste.Narrative.Nodes;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddCharacterInfo", menuName = "Celeste/Twine/Parser Steps/Try Add Character Info")]
    public class TryAddCharacterInfo : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is ICharacterNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineNode node = parseContext.TwineNode;
            ICharacterNode characterNode = parseContext.FSMNode as ICharacterNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            Character character = importerSettings.FindCharacterInTags(node.Tags);
            UnityEngine.Debug.Assert(character != null, $"Could not find character for node {node.Name} ({node.pid}).");
            UIPosition characterDefaultPosition = character != null ? character.DefaultUIPosition : UIPosition.Centre;

            CharacterNodeBuilder.
                WithNode(characterNode).
                WithUIPosition(characterDefaultPosition).
                WithCharacter(character);
        }
    }
}