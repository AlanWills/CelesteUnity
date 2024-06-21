using Celeste.FSM.Nodes.Events.Conditions;
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
    [CreateNodeMenu("Celeste/Events/Queues/Multi Event Queue")]
    [NodeTint(0.8f, 0.9f, 0)]
    public class MultiEventQueueNode : MultiEventNode
    {
        #region Properties and Fields

        private Queue<Tuple<EventCondition, object>> eventQueue = new Queue<Tuple<EventCondition, object>>();

        #endregion

        public MultiEventQueueNode()
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
                    object arg = eventCondition.ConsumeEvent();
                    eventQueue.Enqueue(new Tuple<EventCondition, object>(eventCondition, arg));

                    Debug.LogFormat("Name: {0} with Argument: {1} was queued by MEQ Node", eventCondition.name, argument != null ? argument : "null");
                }
            }

            if (eventQueue.Count > 0)
            {
                Tuple<EventCondition, object> eventCondition = eventQueue.Dequeue();
                argument = eventCondition.Item2;
                
                Debug.LogFormat("Name: {0} with Argument: {1} was consumed by MEQ Node", eventCondition.Item1.name, argument != null ? argument : "null");
                return GetConnectedNodeFromOutput(eventCondition.Item1.name);
            }

            return this;
        }

        #endregion
    }
}
