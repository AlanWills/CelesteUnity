using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.Localisation.Parameters;
using Celeste.Localisation;
using Celeste.Localisation.Settings;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Nodes;
using Celeste.Narrative.UI;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using XNode.Attributes;
using Celeste.Narrative.Tokens;

namespace Celeste.Narrative
{
    [CreateNodeMenu("Celeste/Narrative/Dialogue")]
    [NodeTint(0, 0.4f, 0)]
    public class DialogueNode : NarrativeNode, IInputReceiverNode, IDialogueNode, ICharacterNode
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
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
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
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
            }
        }

        public Object[] DialogueTokens
        {
            set 
            {
                ArrayExtensions.ResizeAndCopyFrom(ref dialogueTokens, value);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
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
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
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
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
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
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
            }
        }

        [HideIf(nameof(isLocalised)), TextRegion(2)] public string dialogue;
        [ShowIf(nameof(isLocalised)), LocalisationPreview] public LocalisationKey localisationKey;
        public bool isLocalised;
        public DialogueType dialogueType = DialogueType.Speech;
        [NodeEnum] public UIPosition uiPosition = UIPosition.Left;
        [HideInNodeEditor] public Object[] dialogueTokens;
        public Character character;
        [ShowIf(nameof(isLocalised))] public LanguageValue currentLanguage;

        [System.NonSerialized] public string tokenizedDialogue;
        [System.NonSerialized] public bool hasBeenLocalised;
        [System.NonSerialized] public bool isRead;

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
            isRead = false;
        }

        protected override FSMNode OnUpdate()
        {
            return isRead ? base.OnUpdate() : this;
        }

        #endregion

        #region IInputReceiverNode

        void IInputReceiverNode.OnContinueInputReceived()
        {
            isRead = true;
        }

        #endregion
    }
} 