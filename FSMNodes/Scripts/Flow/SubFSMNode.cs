using Celeste.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.FSM.Nodes
{
    [CreateNodeMenu("Celeste/Flow/Sub FSM")]
    public class SubFSMNode : FSMNode, ILinearRuntime, IFSMGraph
    {
        #region Properties and Fields

        public FSMNodeUnityEvent OnNodeEnter { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeUpdate { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeExit { get; } = new FSMNodeUnityEvent();

        IFSMGraph IFSMGraph.ParentFSMGraph => FSMGraph;
        FSMNode IFSMGraph.StartNode => StartNode;
        IEnumerable<FSMNode> IFSMGraph.Nodes
        {
            get
            {
                foreach (var node in subFSM.nodes)
                {
                    yield return node as FSMNode;
                }
            }
        }
        ILinearRuntimeRecord ILinearRuntime.Record => FSMGraph.Runtime.Record;

        [NonSerialized] private FSMNode currentNode;
        public FSMNode CurrentNode 
        {
            get => currentNode;
            set
            {
                if (currentNode != value)
                {
                    currentNode = value;
                    FSMGraph.Runtime.Record.CurrentNodePath = new FSMGraphNodePath(currentNode);
                }
            }
        }

        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : subFSM.startNode; }
            set 
            {
                if (startNode != value)
                {
                    startNode = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public FSMGraph SubFSM
        {
            get => subFSM;
            set
            {
                if (subFSM != value)
                {
                    subFSM = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        [SerializeField] private FSMGraph subFSM;

        [NonSerialized] private FSMGraph runtimeSubFSMGraph;
        [NonSerialized] private ILinearRuntime fsmRuntime;
        [NonSerialized] private FSMRuntimeEngine runtimeEngine;

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            if (runtimeSubFSMGraph == null)
            {
                runtimeSubFSMGraph = subFSM.Copy() as FSMGraph;
                runtimeSubFSMGraph.ParentFSMGraph = this;
            }

            fsmRuntime = FSMGraph.Runtime;

            Run();
        }

        protected override FSMNode OnUpdate()
        {
            // When we no longer have nodes to update, we have finished the sub story and can move on
            bool nodesLeftToUpdate = runtimeEngine.Update();
            return nodesLeftToUpdate ? this : GetConnectedNodeFromDefaultOutput();
        }

        protected override void OnExit()
        {
            base.OnExit();

            Stop();
        }

        #endregion

        public void Run()
        {
            OnNodeEnter.AddListener(fsmRuntime.OnNodeEnter.Invoke);
            OnNodeUpdate.AddListener(fsmRuntime.OnNodeUpdate.Invoke);
            OnNodeExit.AddListener(fsmRuntime.OnNodeExit.Invoke);

            runtimeEngine = new FSMRuntimeEngine(this);
            runtimeEngine.Start(runtimeSubFSMGraph.startNode);
        }

        public void Stop()
        {
            OnNodeEnter.RemoveListener(fsmRuntime.OnNodeEnter.Invoke);
            OnNodeUpdate.RemoveListener(fsmRuntime.OnNodeUpdate.Invoke);
            OnNodeExit.RemoveListener(fsmRuntime.OnNodeExit.Invoke);

            fsmRuntime = null;
            runtimeEngine = null;
            currentNode = null;
            startNode = null;
        }

        public FSMNode FindNode(string nodeGuid)
        {
            return subFSM.FindNode(nodeGuid);
        }
    }
}