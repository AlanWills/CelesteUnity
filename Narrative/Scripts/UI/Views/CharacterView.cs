using Celeste.FSM;
using Celeste.Narrative.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Character View")]
    public class CharacterView : NarrativeView
    {
        #region Properties and Fields

        [SerializeField] private GameObject characterNameRoot;
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

            characterAvatarUI.SetActive(character.CharacterAvatarIcon != null);
            characterNameRoot.SetActive(true);
        }

        public override void OnNodeUpdate(FSMNode fsmNode) { }

        public override void OnNodeExit(FSMNode fsmNode)
        {
            characterNameRoot.SetActive(false);
        }

        #endregion
    }
}
