using Celeste.Events;
using System;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [Serializable]
    public abstract class ParameterizedEventCondition<T, TEvent> : EventCondition, IEventListener<T>
        where TEvent : ParameterisedEvent<T>
    {
        #region Properties and Fields

        public TEvent listenFor;

        public override string name
        {
            get { return listenFor != null ? listenFor.name : ""; }
        }

        #endregion

        #region Listeners

        protected override void AddListenerInternal()
        {
            listenFor.AddListener(this);
        }

        protected override void RemoveListenerInternal()
        {
            listenFor.RemoveListener(this);
        }

        public void OnEventRaised(T arg)
        {
            RegisterEventRaised(arg);
        }

        #endregion

        #region ICopyable

        public override void CopyFrom(EventCondition original)
        {
            ParameterizedEventCondition<T, TEvent> eventCondition = original as ParameterizedEventCondition<T, TEvent>;
            eventCondition.listenFor = listenFor;
        }

        #endregion
    }
}
