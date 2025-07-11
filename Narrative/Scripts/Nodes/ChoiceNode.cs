using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.Localisation.Parameters;
using Celeste.Localisation;
using Celeste.Localisation.Settings;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Choices;
using Celeste.Narrative.UI;
using Celeste.Tools.Attributes.GUI;
using System.Collections.Generic;
using UnityEngine;
using XNode.Attributes;
using Celeste.Narrative.Tokens;
using Celeste.Tools;

namespace Celeste.Narrative
{
    [CreateNodeMenu("Celeste/Narrative/Choice")]
    [NodeTint(0, 0, 1f), NodeWidth(250)]
    public class ChoiceNode : NarrativeNode, IChoiceNode, IDialogueNode
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

        public int NumChoices => choices.Count;

        [HideIf(nameof(isLocalised)), TextRegion(2)] public string dialogue;
        [ShowIf(nameof(isLocalised)), LocalisationPreview] public LocalisationKey localisationKey;
        public bool isLocalised;
        public DialogueType dialogueType = DialogueType.Speech;
        [NodeEnum] public UIPosition uiPosition;
        [HideInNodeEditor, SerializeField] private List<LocaToken> dialogueTokens = new();
        public Character character;
        [ShowIf(nameof(isLocalised))] public LanguageValue currentLanguage;
        [SerializeField] private List<Choice> choices = new List<Choice>();

        [System.NonSerialized] private string tokenizedDialogue;
        [System.NonSerialized] private bool hasBeenLocalised;
        [System.NonSerialized] private IChoice selectedChoice;

        #endregion

        public ChoiceNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);
        }

        #region Add/Remove/Copy

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

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            ChoiceNode choiceNode = original as ChoiceNode;

            // The references to the original node's choice will have been copied so we have to clear them and re-add our clones
            choices.Clear();

            for (int i = 0; i < choiceNode.NumChoices; ++i)
            {
                Choice originalChoice = choiceNode.GetChoice(i);
                Choice newChoice = Instantiate(originalChoice);
                newChoice.name = originalChoice.name;
                newChoice.CopyFrom(originalChoice);
                choices.Add(newChoice);
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            for (int i = NumChoices; i > 0; --i)
            {
                RemoveChoice(GetChoice(i - 1));
            }
        }

        #endregion

        #region Choice Management

        public T AddChoice<T>(string name) where T : Choice
        {
            return AddChoice(name, typeof(T)) as T;
        }

        public Choice AddChoice(string name, System.Type type)
        {
            Choice choice = ScriptableObject.CreateInstance(type) as Choice;
            choice.name = name;
            choice.hideFlags = HideFlags.HideInHierarchy;
            choice.ID = name;
            choices.Add(choice);

            if (!Application.isPlaying)
            {
                EditorOnly.AddObjectToAsset(choice, graph);
            }

            AddOutputPort(name);

            return choice;
        }

        public void RemoveChoice(Choice choice)
        {
            if (HasPort(choice.name))
            {
                RemoveDynamicPort(choice.name);
            }

            choices.Remove(choice);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(choice);
            UnityEditor.AssetDatabase.SaveAssets();
            
            DestroyImmediate(choice);
#else
            Destroy(choice);
#endif
        }

        public Choice GetChoice(int index)
        {
            return choices.Get(index);
        }

        public T FindChoice<T>(System.Predicate<T> predicate) where T : Choice
        {
            return choices.Find(x => x is T && predicate(x as T)) as T;
        }

        public void SelectChoice(IChoice choice)
        {
            selectedChoice = choice;
            choice.OnSelected();
        }

        public void MoveChoice(int currentIndex, int newIndex)
        {
            Choice choice = choices.Get(currentIndex);
            choices[currentIndex] = choices[newIndex];
            choices[newIndex] = choice;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
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
            selectedChoice = null;
        }

        protected override FSMNode OnUpdate()
        {
            return selectedChoice != null ? GetConnectedNodeFromOutput(selectedChoice.name) : this;
        }

        #endregion
    }
}
