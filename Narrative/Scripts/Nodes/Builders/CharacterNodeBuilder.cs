using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;

namespace CelesteEditor.Narrative.Nodes
{
    public struct CharacterNodeBuilder
    {
        #region Properties and Fields

        private ICharacterNode characterNode;

        #endregion

        #region Constructor

        private CharacterNodeBuilder(ICharacterNode characterNode)
        {
            this.characterNode = characterNode;
        }

        public static CharacterNodeBuilder WithNode(ICharacterNode characterNode)
        {
            return new CharacterNodeBuilder(characterNode);
        }

        #endregion

        public CharacterNodeBuilder WithUIPosition(UIPosition position)
        {
            characterNode.UIPosition = position;
            return this;
        }

        public CharacterNodeBuilder WithCharacter(Character character)
        {
            characterNode.Character = character;
            return this;
        }
    }
}