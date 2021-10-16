using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    public abstract class ParameterisedEventListenerNode<T, TEvent> : FSMNode, IEventListener<T> where TEvent : ParameterisedEvent<T>
    {
        #region Properties and Fields

        [Output]
        public T argument;

        public TEvent listenFor;

        private bool eventRaised = false;

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            eventRaised = false;
            listenFor.AddListener(this);
        }

        protected override FSMNode OnUpdate()
        {
            return eventRaised ? base.OnUpdate() : this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            eventRaised = false;
            listenFor.RemoveListener(this);
        }

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            return argument;
        }

        #endregion

        #region IEventListener Implementation

        public void OnEventRaised(T argument)
        {
            Debug.LogFormat("Event {0} received with argument {1} in {2} in FSM {3}", listenFor.name, argument, name, graph.name);
            eventRaised = true;
            this.argument = argument;
        }

        #endregion
    }
}
