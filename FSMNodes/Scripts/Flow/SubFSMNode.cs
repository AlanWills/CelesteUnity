using System;
using System.Collections;
using UnityEngine;

namespace Celeste.FSM.Nodes
{
    [CreateNodeMenu("Celeste/Flow/Sub FSM")]
    public class SubFSMNode : FSMNode, ILinearRuntime<FSMNode>
    {
        #region Properties and Fields

        public FSMNodeUnityEvent OnNodeEnter { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeUpdate { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeExit { get; } = new FSMNodeUnityEvent();

        [NonSerialized] private FSMNode currentNode;
        public FSMNode CurrentNode 
        {
            get { return currentNode; }
            set
            {
                currentNode = value;
                Debug.Log($"Setting Current Node: {(currentNode != null ? currentNode.name : "null")}.");
            }
        }

        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : subFSM.startNode; }
            set { startNode = value; }
        }

        public FSMGraph subFSM;

        private ILinearRuntime<FSMNode> fsmRuntime;
        private FSMRuntimeEngine runtimeEngine;

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            fsmRuntime = FSMGraph.Runtime;

            OnNodeEnter.AddListener(fsmRuntime.OnNodeEnter.Invoke);
            OnNodeUpdate.AddListener(fsmRuntime.OnNodeUpdate.Invoke);
            OnNodeExit.AddListener(fsmRuntime.OnNodeExit.Invoke);

            runtimeEngine = new FSMRuntimeEngine(this);
            runtimeEngine.Start(subFSM.startNode);
        }

        protected override FSMNode OnUpdate()
        {
            // When we no longer have nodes to update, we have finished the sub story and can move on
            FSMNode newNode = runtimeEngine.Update();
            return newNode != null ? this : GetConnectedNodeFromDefaultOutput();
        }

        protected override void OnExit()
        {
            base.OnExit();

            OnNodeEnter.RemoveListener(fsmRuntime.OnNodeEnter.Invoke);
            OnNodeUpdate.RemoveListener(fsmRuntime.OnNodeUpdate.Invoke);
            OnNodeExit.RemoveListener(fsmRuntime.OnNodeExit.Invoke);

            fsmRuntime = null;
            runtimeEngine = null;
            CurrentNode = null;
            StartNode = null;
        }

        #endregion
    }
}