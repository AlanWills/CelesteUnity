using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.Narrative.Choices;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Celeste.Tools;

namespace Celeste.Narrative
{
    [DisplayName("Choice Node")]
    [NodeTint(0, 0, 1f), NodeWidth(250)]
    public class ChoiceNode : DialogueNode, IChoiceNode
    {
        #region Properties and Fields
        
        public int NumChoices => choices.Count;
        public IReadOnlyList<Choice> Choices => choices;
        
        [SerializeField] private List<Choice> choices = new();

        [System.NonSerialized] private IChoice selectedChoice;

        #endregion

        #region Add/Remove/Copy

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

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();
            
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);
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

        public T AddChoice<T>(string choiceName) where T : Choice
        {
            return AddChoice(choiceName, typeof(T)) as T;
        }

        public Choice AddChoice(string choiceName, System.Type type)
        {
            Choice choice = ScriptableObject.CreateInstance(type) as Choice;
            choice.name = choiceName;
            choice.hideFlags = HideFlags.HideInHierarchy;
            choice.ID = choiceName;
            choice.OnValidate();
            choices.Add(choice);

            if (!Application.isPlaying)
            {
                choice.AddObjectToAsset(graph);
            }

            AddOutputPort(choiceName);

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
            EditorOnly.SetDirty(this);
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();
            
            selectedChoice = null;
        }

        protected override FSMNode OnUpdate()
        {
            return selectedChoice != null ? GetConnectedNodeFromOutput(selectedChoice.name) : this;
        }

        #endregion
    }
}
