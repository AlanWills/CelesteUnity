using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEditor.UIElements;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [NodeWidth(250)]
    public abstract class ParameterisedEventRaiserNode<T, TEvent> : FSMNode
        where TEvent : ParameterisedEvent<T>
    {
        #region Properties and Fields

        public TEvent toRaise;
        public T argument;

        #endregion

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            ParameterisedEventRaiserNode<T, TEvent> eventRaiserNode = original as ParameterisedEventRaiserNode<T, TEvent>;
            toRaise = eventRaiserNode.toRaise;
            argument = eventRaiserNode.argument;
        }

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            toRaise.Invoke(argument);
        }

        #endregion
    }

    [Serializable]
    [NodeWidth(250)]
    public abstract class ParameterisedEventRaiserNode<T, TValue, TReference, TEvent> : FSMNode 
        where TEvent : ParameterisedEvent<T>
        where TValue : IValue<T>
        where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        public TEvent toRaise;

        [Input]
        public TReference argument;

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
            
            if (argument == null && eventRaiserNode.argument != null)
            {
                argument = CreateParameter(eventRaiserNode.argument);
            }

            toRaise = eventRaiserNode.toRaise;
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
