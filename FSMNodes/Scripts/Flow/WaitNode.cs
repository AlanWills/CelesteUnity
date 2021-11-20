using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes
{
    public enum WaitUnit
    {
        Seconds,
        Frames
    }

    [Serializable]
    [CreateNodeMenu("Celeste/Flow/Wait")]
    [NodeTint(0, 0.4f, 0)]
    [NodeWidth(250)]
    public class WaitNode : FSMNode
    {
        #region Properties and Fields

        public FloatReference time;
        public WaitUnit unit = WaitUnit.Seconds;

        private float currentTime = 0;

        #endregion

        #region Add/Remove/Copy

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (time == null)
            {
                time = CreateParameter<FloatReference>(name + "_Time");
                time.IsConstant = true;
                time.Value = 1;
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            RemoveParameter(time);
        }

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            WaitNode originalWaitNode = original as WaitNode;
            time = CreateParameter(originalWaitNode.time);
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            currentTime = 0;
        }

        protected override FSMNode OnUpdate()
        {
            if (unit == WaitUnit.Seconds)
            {
                currentTime += Time.deltaTime;
            }
            else if (unit == WaitUnit.Frames)
            {
                ++currentTime;
            }
            else
            {
                Debug.LogAssertionFormat("Unhandled WaitUnit {0} in WaitNode in graph {1}", unit, graph.name);
            }

            return currentTime >= time.Value ? base.OnUpdate() : this;
        }

        #endregion
    }
}
