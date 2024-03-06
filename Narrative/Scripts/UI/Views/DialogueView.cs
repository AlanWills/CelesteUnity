using Celeste.FSM;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Dialogue View")]
    public class DialogueView : NarrativeView
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private bool hasAnimator;
        [SerializeField, ShowIf(nameof(hasAnimator))] private Animator dialogueAnimator;
        [SerializeField] private DialogueTypeStyle[] dialogueTypeStyles;
        [SerializeField] private List<UIPosition> supportedUIPositions = new List<UIPosition>();

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

            if (supportedUIPositions == null || supportedUIPositions.Count == 0)
            {
                supportedUIPositions = supportedUIPositions ?? new List<UIPosition>();

                foreach (var enumValue in Enum.GetValues(typeof(UIPosition)))
                {
                    supportedUIPositions.Add((UIPosition)enumValue);
                }
            }
        }

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is IDialogueNode && 
                !string.IsNullOrEmpty((fsmNode as IDialogueNode).Dialogue) &&
                supportedUIPositions.Contains((fsmNode as IDialogueNode).UIPosition);
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            IDialogueNode dialogueNode = fsmNode as IDialogueNode;
            dialogueText.text = dialogueNode.Dialogue;

            SetDialogueTypeStyle(dialogueNode.DialogueType);

            if (hasAnimator)
            {
                dialogueAnimator.SetTrigger("Show");
            }
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
    }
}
