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
        [SerializeField] private UIPositionAnchor[] uiPositionAnchors;

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

            bool showCharacter = character.CharacterAvatarIcon != null;
            if (showCharacter)
            {
                SetUIPosition(characterNode.UIPosition);
            }

            characterNameUI.SetActive(true);
            characterAvatarUI.SetActive(showCharacter);
        }

        public override void OnNodeUpdate(FSMNode fsmNode) { }

        public override void OnNodeExit(FSMNode fsmNode)
        {
            characterNameUI.SetActive(false);
        }

        #endregion

        public void SetUIPosition(UIPosition uiPosition)
        {
            UIPositionAnchor positionAnchor = uiPositionAnchors.Find(x => x.uiPosition == uiPosition);
            RectTransform anchor = positionAnchor.anchor;
            UnityEngine.Debug.Assert(anchor != null, $"Could not find anchor for UI Position {uiPosition}.  Perhaps it has not been set in the Inspector?");

            RectTransform avatarTransform = characterAvatarUI.GetComponent<RectTransform>();
            avatarTransform.CopyLayoutFrom(anchor);
        }
    }
}
