using Celeste.FSM;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Dialogue View")]
    public class DialogueView : NarrativeView
    {
        #region Properties and Fields

        [SerializeField] private RectTransform dialogueTransform;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Animator dialogueAnimator;
        [SerializeField] private DialogueTypeStyle[] dialogueTypeStyles;
        [SerializeField] private UIPositionAnchor[] uiPositionAnchors;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref dialogueAnimator);

            if (dialogueTypeStyles == null || dialogueTypeStyles.Length == 0)
            {
                dialogueTypeStyles = new DialogueTypeStyle[3]
                {
                    new DialogueTypeStyle() { dialogueType = DialogueType.Speech, format = "{0}" },
                    new DialogueTypeStyle() { dialogueType = DialogueType.Thinking, format = "<i>{0}</i>" },
                    new DialogueTypeStyle() { dialogueType = DialogueType.Action, format = "<b>{0}</b>" }
                };
            }
        }

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is IDialogueNode && !string.IsNullOrEmpty((fsmNode as IDialogueNode).RawDialogue);
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            IDialogueNode dialogueNode = fsmNode as IDialogueNode;
            dialogueText.text = dialogueNode.Dialogue;

            SetDialogueTypeStyle(dialogueNode.DialogueType);
            SetUIPosition(dialogueNode.UIPosition);

            dialogueAnimator.SetTrigger("Show");
        }

        public override void OnNodeUpdate(FSMNode fsmNode) { }

        public override void OnNodeExit(FSMNode fsmNode) { }

        #endregion

        private void SetDialogueTypeStyle(DialogueType dialogueType)
        {
            for (int i = 0, n = dialogueTypeStyles != null ? dialogueTypeStyles.Length : 0; i < n; ++i)
            {
                DialogueTypeStyle style = dialogueTypeStyles[i];
                
                if ((style.dialogueType & dialogueType) == dialogueType)
                {
                    dialogueText.text = string.Format(style.format, dialogueText.text);
                }
            }
        }

        public void SetUIPosition(UIPosition uiPosition)
        {
            UIPositionAnchor positionAnchor = Array.Find(uiPositionAnchors, x => x.uiPosition == uiPosition);
            RectTransform anchor = positionAnchor.anchor;
            UnityEngine.Debug.Assert(anchor != null, $"Could not find anchor for UI Position {uiPosition}.  Perhaps it has not been set in the Inspector?");

            dialogueTransform.anchoredPosition = anchor.anchoredPosition;
            dialogueTransform.pivot = anchor.pivot;
            dialogueTransform.localPosition = anchor.localPosition;
        }
    }
}
