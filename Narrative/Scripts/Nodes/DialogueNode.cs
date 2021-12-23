using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Tokens;
using Celeste.Narrative.UI;
using Celeste.Tools.Attributes.GUI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using XNode.Attributes;

namespace Celeste.Narrative
{
    [CreateNodeMenu("Celeste/Narrative/Dialogue")]
    [NodeTint(0, 0.4f, 0)]
    public class DialogueNode : NarrativeNode, IPointerClickHandler, IDialogueNode, ICharacterNode
    {
        #region Properties and Fields

        Vector2 IDialogueNode.Position 
        {
            get { return position; }
            set { position = value; }
        }

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

        public ScriptableObject[] DialogueTokens
        {
            set { ArrayExtensions.ResizeAndCopyFrom(ref dialogueTokens, value); }
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
            get { return uiPosition; }
            set
            {
                uiPosition = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        [SerializeField, TextArea] private string dialogue;
        [SerializeField] private DialogueType dialogueType = DialogueType.Speech;
        [SerializeField, NodeEnum] private UIPosition uiPosition;
        [SerializeField, HideInNodeEditor] private ScriptableObject[] dialogueTokens;
        [SerializeField] private Character character;

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

        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            isRead = true;
        }

        #endregion
    }
} 