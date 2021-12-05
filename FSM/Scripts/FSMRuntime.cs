using System;
using UnityEngine;
using XNode;

namespace Celeste.FSM
{
    [AddComponentMenu("Celeste/FSM/FSM Runtime")]
    public class FSMRuntime : SceneGraph<FSMGraph>, ILinearRuntime<FSMNode>
    {
        #region Properties and Fields

        public FSMNodeUnityEvent OnNodeEnter { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeUpdate { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeExit { get; } = new FSMNodeUnityEvent();

        public FSMNode CurrentNode { get; set; }

        // Runtime only override of the start node - useful for loading an FSM at a particular state
        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : graph.startNode; }
            set { startNode = value; }
        }

        [SerializeField]
        private bool lateUpdate = false;

        private FSMRuntimeEngine runtimeEngine;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (graph != null)
            {
                graph.Runtime = this;

                runtimeEngine = new FSMRuntimeEngine(this);
                runtimeEngine.Start(StartNode);
            }

            if (CurrentNode != null)
            {
                Debug.LogFormat("Spooling up FSM with starting node {0}", CurrentNode.name);
            }
            else 
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (!lateUpdate)
            {
                runtimeEngine.Update();
            }
        }

        private void LateUpdate()
        {
            if (lateUpdate)
            {
                runtimeEngine.Update();
            }
        }

        #endregion
    }
}
