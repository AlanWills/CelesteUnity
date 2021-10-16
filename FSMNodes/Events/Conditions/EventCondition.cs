using Celeste.Events;
using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [Serializable]
    public abstract class EventCondition : ScriptableObject, ICopyable<EventCondition>
    {
        #region Properties and Fields

        public abstract new string name { get; }

        private Queue<object> arguments = new Queue<object>();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            hideFlags = HideFlags.HideInHierarchy;
        }

        #endregion

        #region Listeners

        public void AddListener()
        {
            AddListenerInternal();
        }

        public void RemoveListener()
        {
            RemoveListenerInternal();
        }

        protected abstract void AddListenerInternal();

        protected abstract void RemoveListenerInternal();

        #endregion

        #region Event Firing

        public bool HasEventFired()
        {
            return arguments.Count > 0;
        }

        public void RegisterEventRaised(object argument)
        {
            arguments.Enqueue(argument);
        }

        public object ConsumeEvent()
        {
            Debug.Assert(HasEventFired());
            return HasEventFired() ? arguments.Dequeue() : null;
        }

        #endregion

        #region ICopyable

        public abstract void CopyFrom(EventCondition original);

        #endregion
    }
}
