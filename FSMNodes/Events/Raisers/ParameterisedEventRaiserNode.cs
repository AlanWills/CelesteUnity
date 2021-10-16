using Celeste.Events;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [NodeWidth(250)]
    public abstract class ParameterisedEventRaiserNode<T, TValue, TReference, TEvent> : FSMNode 
        where TEvent : ParameterisedEvent<T>
        where TValue : ParameterValue<T>
        where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        [Input]
        public TReference argument;

        public TEvent toRaise;

        #endregion

        #region Add/Remove/Copy

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (argument == null)
            {
                argument = CreateParameter<TReference>(name + "_argument");
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            RemoveParameter(argument);
        }

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            ParameterisedEventRaiserNode<T, TValue, TReference, TEvent> eventRaiserNode = original as ParameterisedEventRaiserNode<T, TValue, TReference, TEvent>;
            argument = CreateParameter(eventRaiserNode.argument);
        }

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            TReference _argument = GetInputValue(nameof(argument), argument);
            T value = _argument != null ? _argument.Value : GetInputValue(nameof(argument), argument.Value);
            toRaise.Invoke(value);
        }

        #endregion
    }
}
