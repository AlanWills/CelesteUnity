using Celeste.FSM.Nodes.Events.Conditions;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [CreateNodeMenu("Celeste/Events/Listeners/Multi Event Listener")]
    [NodeTint(0.8f, 0.9f, 0)]
    public class MultiEventListenerNode : MultiEventNode
    {
        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();
            
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);
        }

        #region FSM Runtime Methods

        protected override FSMNode OnUpdate()
        {
            foreach (EventCondition eventCondition in this)
            {
                if (eventCondition.HasEventFired())
                {
                    string eventConditionName = eventCondition.name;
                    argument = eventCondition.ConsumeEvent();

                    Debug.Log($"Name: {eventConditionName} with Argument: {argument ?? string.Empty} was consumed by MEL Node.");
                    return GetConnectedNodeFromOutput(eventConditionName);
                }
            }

            return this;
        }

        #endregion
    }
}
