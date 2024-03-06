using System;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Celeste.FSM
{
    [AddComponentMenu("Celeste/FSM/FSM Runtime")]
    public class FSMRuntime : SceneGraph<FSMGraph>, ILinearRuntime<FSMNode>
    {
        #region Properties and Fields

        public FSMNodeUnityEvent OnNodeEnter => onNodeEnter;
        public FSMNodeUnityEvent OnNodeUpdate => onNodeUpdate;
        public FSMNodeUnityEvent OnNodeExit => onNodeExit;
        public UnityEvent OnFinished => onFinished;

        public ILinearRuntimeRecord Record { get; } = new FSMRecord();

        public FSMNode CurrentNode { get; set; }

        // Runtime only override of the start node - useful for loading an FSM at a particular state
        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : graph.startNode; }
            set { startNode = value; }
        }

        [SerializeField] private bool startAutomatically = true;
        [SerializeField] private bool lateUpdate = false;
        [SerializeField] private FSMNodeUnityEvent onNodeEnter = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeUpdate = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeExit = new FSMNodeUnityEvent();
        [SerializeField] private UnityEvent onFinished = new UnityEvent();

        private FSMRuntimeEngine runtimeEngine;

        #endregion

        public void StartFSM()
        {
            CurrentNode = null;
            enabled = true;

            if (graph != null)
            {
                graph.Runtime = this;

                runtimeEngine = new FSMRuntimeEngine(this);
                runtimeEngine.Start(StartNode);
            }

            if (CurrentNode == null)
            {
                StopFSM();
            }
        }

        public void StopFSM()
        {
            enabled = false;
            runtimeEngine = null;
            CurrentNode = null;
        }

        private void UpdateFSM()
        {
            if (runtimeEngine != null && runtimeEngine.Update() == graph.finishNode)
            {
                OnFinished.Invoke();
                StopFSM();
            }
        }

        #region Unity Methods

        private void Start()
        {
            if (startAutomatically == false)
            {
                return;
            }

            StartFSM();
        }

        private void Update()
        {
            if (!lateUpdate)
            {
                UpdateFSM();
            }
        }

        private void LateUpdate()
        {
            if (lateUpdate)
            {
                UpdateFSM();
            }
        }

        #endregion
    }
}
