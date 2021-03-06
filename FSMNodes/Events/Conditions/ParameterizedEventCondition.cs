﻿using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [Serializable]
    public class ParameterizedEventCondition<T, TEvent> : EventCondition, IEventListener<T>
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
            listenFor.AddEventListener(this);
        }

        protected override void RemoveListenerInternal()
        {
            listenFor.RemoveEventListener(this);
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
