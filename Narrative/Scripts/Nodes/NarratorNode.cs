using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celeste.FSM;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Settings;
using Celeste.Narrative.Tokens;
using Celeste.Narrative.UI;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using XNode.Attributes;

namespace Celeste.Narrative
{
    [Obsolete]
    [NodeTint(0, 0.4f, 0)]
    public class NarratorNode : NarrativeNode, IDialogueNode, ICharacterNode
    {
        #region Properties and Fields

        Vector2 IDialogueNode.Position 
        {
            get => position;
            set => position = value;
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

        public IReadOnlyList<LocaToken> DialogueTokens
        {
            set
            {
                dialogueTokens.Clear();
                dialogueTokens.AddRange(value);
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
            set { }
        }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        [SerializeField, TextRegion(2)] private string dialogue;
        [SerializeField, HideInNodeEditor] private DialogueType dialogueType = DialogueType.Speech;
        [HideInNodeEditor, SerializeField] private List<LocaToken> dialogueTokens = new();
        [SerializeField, HideInNodeEditor] private Character character;

        private string tokenizedDialogue;

        #endregion
        
        #region Unity Methods

        private void OnValidate()
        {
            TrySetNarratorCharacter();
        }

        #endregion

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();
            
            TrySetNarratorCharacter();
        }

        [Conditional("UNITY_EDITOR")]
        private void TrySetNarratorCharacter()
        {
#if UNITY_EDITOR
            if (character == null)
            {
                character = NarrativeEditorSettings.GetOrCreateSettings().narratorCharacter;
                EditorOnly.SetDirty(this);
            }
#endif
        }

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            tokenizedDialogue = TokenUtility.SubstituteTokens(dialogue, dialogueTokens);
        }

        protected override FSMNode OnUpdate()
        {
            return this;
        }

        #endregion
    }
}
