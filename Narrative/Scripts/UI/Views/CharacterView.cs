using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.Narrative.Characters;
using Celeste.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Character View")]
    public class CharacterView : NarrativeView
    {
        #region Properties and Fields

        [SerializeField] private GameObject characterNameUI;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private GameObject characterAvatarUI;
        [SerializeField] private Image characterAvatarIcon;

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is ICharacterNode;
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            ICharacterNode characterNode = fsmNode as ICharacterNode;
            Character character = characterNode.Character;
            characterName.text = character.CharacterName;
            characterAvatarIcon.sprite = character.CharacterAvatarIcon;

            characterNameUI.SetActive(true);
            characterAvatarUI.SetActive(character.CharacterAvatarIcon != null);
        }

        public override void OnNodeUpdate(FSMNode fsmNode) { }

        public override void OnNodeExit(FSMNode fsmNode)
        {
            characterNameUI.SetActive(false);
        }

        #endregion
    }
}
