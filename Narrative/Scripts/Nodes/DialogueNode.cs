using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.Localisation;
using Celeste.Localisation.Parameters;
using Celeste.Localisation.Settings;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Tokens;
using Celeste.Narrative.UI;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using UnityEngine.EventSystems;
using XNode.Attributes;

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
            get { return uiPosition; }
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

        [SerializeField, HideIf(nameof(isLocalised)), TextRegion(2)] private string dialogue;
        [SerializeField, ShowIf(nameof(isLocalised)), LocalisationPreview] private LocalisationKey localisationKey;
        [SerializeField] private bool isLocalised;
        [SerializeField] private DialogueType dialogueType = DialogueType.Speech;
        [SerializeField, NodeEnum] private UIPosition uiPosition;
        [SerializeField, HideInNodeEditor] private Object[] dialogueTokens;
        [SerializeField] private Character character;
        [SerializeField, ShowIf(nameof(isLocalised))] private LanguageValue currentLanguage;

        [System.NonSerialized] private string tokenizedDialogue;
        [System.NonSerialized] private bool hasBeenLocalised = false;
        [System.NonSerialized] private bool isRead = false;

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

            UnityEngine.Debug.Assert(!isLocalised || currentLanguage != null, $"Current Language has not been set in Dialogue node {name}.");
            UnityEngine.Debug.Assert(!isLocalised || localisationKey != null, $"Localisation Key has not been set in Dialogue node {name}.");

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