using Celeste.FSM;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Characters.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Character 2D Avatar View")]
    public class Character2DAvatarView : NarrativeView
    {
        #region Properties and Fields
        
        [SerializeField] private GameObject characterAvatarUI;
        [SerializeField] private Image characterAvatarIcon;

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is ICharacterNode characterNode && 
                   characterNode.Character != null &&
                   characterNode.Character.HasComponent<Character2DArtComponent>();
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            ICharacterNode characterNode = fsmNode as ICharacterNode;
            Character character = characterNode.Character;
            Character2DArtComponent character2DArtComponent = character.FindComponent<Character2DArtComponent>();
            characterAvatarIcon.sprite = character2DArtComponent.GetSpriteForExpression(characterNode.Expression);
        }

        public override void OnNodeUpdate(FSMNode fsmNode) { }

        public override void OnNodeExit(FSMNode fsmNode)
        {
        }

        #endregion
    }
}
