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

        public string Dialogue => tokenizedDialogue;

        public string RawDialogue
        {
            get => dialogue;
            set
            {
                dialogue = value;
                EditorOnly.SetDirty(this);
            }
        }

        public IReadOnlyList<LocaToken> DialogueTokens
        {
            set
            {
                dialogueTokens.Clear();
                dialogueTokens.AddRange(value);
                EditorOnly.SetDirty(this);
            }
        }

        public DialogueType DialogueType
        {
            get => dialogueType;
            set
            {
                dialogueType = value;
                EditorOnly.SetDirty(this);
            }
        }

        public UIPosition UIPosition
        {
            get => UIPosition.Narrator;
            set { }
        }

        public Character Character
        {
            get => character;
            set => character = value;
        }
        
        public string Expression => string.Empty;

        [SerializeField, TextRegion(2)] private string dialogue;
        [SerializeField, HideInNodeEditor] private DialogueType dialogueType = DialogueType.Speech;
        [HideInNodeEditor, SerializeField] private List<LocaToken> dialogueTokens = new();
        [SerializeField, HideInNodeEditor] private Character character;

        private string tokenizedDialogue;

        #endregion
        
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
