using Celeste.FSM;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Characters.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Character Name View")]
    public class CharacterNameView : NarrativeView
    {
        #region Properties and Fields

        [SerializeField] private GameObject characterNameUI;
        [SerializeField] private TextMeshProUGUI characterName;

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is ICharacterNode characterNode && characterNode.Character != null;
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            ICharacterNode characterNode = fsmNode as ICharacterNode;
            Character character = characterNode.Character;
            characterName.text = character.CharacterName;
        }

        public override void OnNodeUpdate(FSMNode fsmNode) { }

        public override void OnNodeExit(FSMNode fsmNode)
        {
            characterNameUI.SetActive(false);
        }

        #endregion
    }
}
