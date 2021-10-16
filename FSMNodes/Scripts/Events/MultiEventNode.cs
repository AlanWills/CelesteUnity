using Celeste.FSM.Nodes.Events.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.FSM.Nodes.Events
{
    public abstract class MultiEventNode : FSMNode, IEnumerable<EventCondition>
    {
        #region Properties and Fields

        [Output]
        public object argument;

        [SerializeField]
        private List<EventCondition> events = new List<EventCondition>();

        public uint NumEvents
        {
            get { return (uint)events.Count; }
        }

        #endregion

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            MultiEventNode originalListenerNode = original as MultiEventNode;

            for (uint i = 0; i < originalListenerNode.NumEvents; ++i)
            {
                EventCondition originalCondition = originalListenerNode.GetEvent(i);
                EventCondition newCondition = AddEvent(originalCondition.GetType());
                newCondition.CopyFrom(originalCondition);
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            for (uint i = NumEvents; i > 0; --i)
            {
                RemoveEvent(GetEvent(i - 1));
            }
        }

        #endregion

        #region Event Condition Utilities

        public EventCondition GetEvent(uint index)
        {
            return index < NumEvents ? events[(int)index] : null;
        }

        public T AddEvent<T>() where T : EventCondition
        {
            return AddEvent(typeof(T)) as T;
        }

        public EventCondition AddEvent(Type conditionType)
        {
            EventCondition _event = ScriptableObject.CreateInstance(conditionType) as EventCondition;
            events.Add(_event);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.AddObjectToAsset(_event, graph);
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(graph);
#endif

            return _event;
        }

        public void RemoveEvent(EventCondition eventCondition)
        {
            bool hasPort = HasPort(eventCondition.name);
            Debug.Assert(hasPort, string.Format("Missing port {0} for event condition being removed.", eventCondition.name));

            if (hasPort)
            {
                RemoveDynamicPort(eventCondition.name);
            }

            events.Remove(eventCondition);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(eventCondition);
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(graph);
#endif
        }

        public NodePort AddEventConditionPort(string name)
        {
            AddOutputPort(name);
            return GetOutputPort(name);
        }

        public void SwapEvents(uint firstIndex, uint secondIndex)
        {
            EventCondition temp = events[(int)firstIndex];
            events[(int)firstIndex] = events[(int)secondIndex];
            events[(int)secondIndex] = temp;
        }

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            return argument;
        }

        #endregion

        #region IEnumerable

        public IEnumerator<EventCondition> GetEnumerator()
        {
            return events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return events.GetEnumerator();
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            foreach (EventCondition eventCondition in this)
            {
                eventCondition.AddListener();
            }
        }

        protected override void OnExit()
        {
            base.OnExit();

            foreach (EventCondition eventCondition in this)
            {
                eventCondition.RemoveListener();
            }
        }

        #endregion
    }
}
