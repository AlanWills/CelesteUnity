using System.Collections.Generic;
using System.ComponentModel;
using Celeste.FSM;
using Celeste.Localisation.Parameters;
using Celeste.Localisation;
using Celeste.Localisation.Settings;
using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using XNode.Attributes;
using Celeste.Narrative.Tokens;
using Celeste.Tools;

namespace Celeste.Narrative
{
    [DisplayName("Dialogue Node")]
    [NodeTint(0, 0.4f, 0)]
    public class DialogueNode : NarrativeNode, IDialogueNode, ICharacterNode
    {
        #region Properties and Fields

        Vector2 IDialogueNode.Position 
        {
            get => position;
            set 
            {
                if (position != value)
                {
                    position = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public string Dialogue => tokenizedDialogue;

        public string RawDialogue
        {
            get => dialogue;
            set
            {
                if (dialogue != value)
                {
                    dialogue = value;
                    EditorOnly.SetDirty(this);
                }
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
                if (dialogueType != value)
                {
                    dialogueType = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public UIPosition UIPosition
        {
            get => uiPosition;
            set
            {
                if (uiPosition != value)
                {
                    uiPosition = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public Character Character
        {
            get => character;
            set
            {
                if (character != value)
                {
                    character = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }
        
        public string Expression => expression;

        [SerializeField, HideIf(nameof(isLocalised)), TextRegion(2)] private string dialogue;
        [SerializeField, ShowIf(nameof(isLocalised)), LocalisationPreview] private LocalisationKey localisationKey;
        [SerializeField, ShowIf(nameof(isLocalised))] private LanguageValue currentLanguage;
        [SerializeField] private DialogueType dialogueType = DialogueType.Speech;
        [SerializeField, NodeEnum] private UIPosition uiPosition = UIPosition.Left;
        [HideInNodeEditor, SerializeField] private List<LocaToken> dialogueTokens = new();
        [SerializeField] private Character character;
        [SerializeField] private string expression;
        [SerializeField] private bool isLocalised;

        [System.NonSerialized] private string tokenizedDialogue;
        [System.NonSerialized] private bool hasBeenLocalised;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (isLocalised && currentLanguage == null)
            {
                currentLanguage = LocalisationEditorSettings.GetOrCreateSettings().currentLanguageValue;
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        protected override void Init()
        {
            base.Init();

            hasBeenLocalised = false;
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            UnityEngine.Debug.Assert(!isLocalised || currentLanguage != null, $"Current Language has not been set.");
            UnityEngine.Debug.Assert(!isLocalised || localisationKey != null, $"Localisation Key has not been set.");

            if (isLocalised &&
                currentLanguage != null &&
                localisationKey != null)
            {
                if (!hasBeenLocalised || string.IsNullOrEmpty(tokenizedDialogue))
                {
                    tokenizedDialogue = currentLanguage.Localise(localisationKey);
                    hasBeenLocalised = true;
                }
            }
            else
            {
                tokenizedDialogue = dialogue;
            }

            tokenizedDialogue = TokenUtility.SubstituteTokens(tokenizedDialogue, dialogueTokens);
        }

        protected override FSMNode OnUpdate()
        {
            return this;
        }

        #endregion
        
        #region Copy Methods

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);
            
            DialogueNode dialogueNode = original as DialogueNode;
            dialogue = dialogueNode.dialogue;
            dialogueType = dialogueNode.dialogueType;
            uiPosition = dialogueNode.UIPosition;
            dialogueTokens.AddRange(dialogueNode.dialogueTokens);
            character = dialogueNode.character;
        }

        #endregion
    }
} 