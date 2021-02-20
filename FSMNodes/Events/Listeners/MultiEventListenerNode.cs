using Celeste.Events;
using Celeste.FSM.Nodes.Events.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;
using static XNode.Node;

using Event = Celeste.Events.Event;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [CreateNodeMenu("Celeste/Events/Listeners/Multi Event Listener")]
    [NodeTint(0.8f, 0.9f, 0)]
    public class MultiEventListenerNode : MultiEventNode
    {
        public MultiEventListenerNode()
        {
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

                    Debug.LogFormat("Name: {0} with Argument: {1} was consumed by MEL Node", eventConditionName, argument != null ? argument : "");
                    return GetConnectedNode(eventConditionName);
                }
            }

            return this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            foreach (EventCondition eventCondition in this)
            {
                // Consume all invocations of events - completely refresh
                while (eventCondition.HasEventFired())
                {
                    Debug.LogFormat("Name: {0} silently consumed by MEL Node", eventCondition.name);
                    eventCondition.ConsumeEvent();
                }
            }
        }

        #endregion
    }
}
