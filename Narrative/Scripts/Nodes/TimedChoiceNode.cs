using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.Narrative.Choices;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Narrative
{
    [DisplayName("Timed Choice")]
    [NodeTint(0, 0, 1f)]
    public class TimedChoiceNode : DialogueNode, IChoiceNode, ITimedNode
    {
        #region Properties and Fields

        public float AllowedTime => allowedTime;
        public float ElapsedTime => elapsedTime;
        public int NumChoices => choices.Count;

        [SerializeField] private float allowedTime = 4;
        [SerializeField] private List<Choice> choices = new();

        private IChoice selectedChoice;
        private float elapsedTime;

        #endregion

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            TimedChoiceNode timedChoiceNode = original as TimedChoiceNode;
            allowedTime = timedChoiceNode.allowedTime;

            for (int i = 0; i < timedChoiceNode.NumChoices; ++i)
            {
                Choice originalChoice = timedChoiceNode.GetChoice(i);
                Choice newChoice = AddChoice(originalChoice.name, originalChoice.GetType());
                newChoice.CopyFrom(originalChoice);
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

        public Choice AddChoice(string name, Type type)
        {
            Choice choice = ScriptableObject.CreateInstance(type) as Choice;
            choice.name = name;
            choice.hideFlags = HideFlags.HideInHierarchy;
            choice.ID = name;
            choices.Add(choice);

            if (!Application.isPlaying)
            {
                choice.AddObjectToAsset(graph);
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

        public T FindChoice<T>(Predicate<T> predicate) where T : Choice
        {
            return choices.Find(x => x is T && predicate(x as T)) as T;
        }

        public void SelectChoice(IChoice choice)
        {
            selectedChoice = choice;
            choice.OnSelected();
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            elapsedTime = 0;
            selectedChoice = null;
        }

        protected override FSMNode OnUpdate()
        {
            if (selectedChoice != null)
            {
                // Made a choice, so we can move on now!
                return GetConnectedNodeFromOutput(selectedChoice.name);
            }

            elapsedTime += Time.deltaTime;

            if (elapsedTime > allowedTime)
            {
                // Time is up - let's use the default port to move on
                return GetConnectedNodeFromDefaultOutput();
            }

            // We've still got time left, so lets continue on this node
            return this;
        }

        #endregion
    }
}
