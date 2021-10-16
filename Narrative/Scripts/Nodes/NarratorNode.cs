using Celeste.FSM;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Tokens;
using Celeste.Narrative.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using XNode.Attributes;
using static XNode.Node;

namespace Celeste.Narrative
{
    [CreateNodeMenu("Celeste/Narrative/Narrator")]
    [NodeTint(0, 0.4f, 0)]
    public class NarratorNode : NarrativeNode, IPointerClickHandler, IDialogueNode, ICharacterNode
    {
        #region Properties and Fields

        public string Dialogue
        {
            get { return tokenizedDialogue; }
        }

        public string RawDialogue
        {
            get { return dialogue; }
            set
            {
                dialogue = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public DialogueType DialogueType
        {
            get { return dialogueType; }
            set
            {
                dialogueType = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public UIPosition UIPosition
        {
            get { return UIPosition.Narrator; }
        }

        public Character Character
        {
            get { return character; }
        }

        [SerializeField, TextArea] private string dialogue;
        [SerializeField, HideInNodeEditor] private DialogueType dialogueType = DialogueType.Speech;
        [SerializeField, HideInNodeEditor] private ScriptableObject[] dialogueTokens;
        [SerializeField, HideInNodeEditor] private Character character;

        private string tokenizedDialogue;
        private bool isRead = false;

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            tokenizedDialogue = TokenUtility.SubstituteTokens(dialogue, dialogueTokens);
            isRead = false;
        }

        protected override FSMNode OnUpdate()
        {
            return isRead ? base.OnUpdate() : this;
        }

        #endregion

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            isRead = true;
        }
    }
}
