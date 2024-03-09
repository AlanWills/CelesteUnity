using System;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Celeste.FSM
{
    [AddComponentMenu("Celeste/FSM/FSM Runtime")]
    public class FSMRuntime : SceneGraph<FSMGraph>, ILinearRuntime
    {
        #region Properties and Fields

        public UnityEvent<ILinearRuntime> OnRun => onRun;
        public UnityEvent<ILinearRuntime> OnStop => onStop;
        public FSMNodeUnityEvent OnNodeEnter => onNodeEnter;
        public FSMNodeUnityEvent OnNodeUpdate => onNodeUpdate;
        public FSMNodeUnityEvent OnNodeExit => onNodeExit;

        ILinearRuntimeRecord ILinearRuntime.Record => record;

        public FSMNode CurrentNode { get; set; }

        // Runtime only override of the start node - useful for loading an FSM at a particular state
        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : graph.startNode; }
            set { startNode = value; }
        }

        [SerializeField] private UnityEvent<ILinearRuntime> onRun = new UnityEvent<ILinearRuntime>();
        [SerializeField] private UnityEvent<ILinearRuntime> onStop = new UnityEvent<ILinearRuntime>();
        [SerializeField] private FSMNodeUnityEvent onNodeEnter = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeUpdate = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeExit = new FSMNodeUnityEvent();
        [SerializeField] private bool startAutomatically = true;
        [SerializeField] private bool lateUpdate = false;

        protected ILinearRuntimeRecord record;
        private FSMRuntimeEngine runtimeEngine;

        #endregion

        public void Run()
        {
            CurrentNode = null;
            enabled = true;

            if (graph != null)
            {
                graph.Runtime = this;
                OnRun.Invoke(this);

                runtimeEngine = new FSMRuntimeEngine(this);
                runtimeEngine.Start(StartNode);
            }

            if (CurrentNode == null)
            {
                Stop();
            }
        }

        public void Stop()
        {
            OnStop.Invoke(this);
            
            enabled = false;
            runtimeEngine = null;
            CurrentNode = null;
        }

        private void UpdateFSM()
        {
            if (runtimeEngine == null)
            {
                return;
            }

            if (CurrentNode == null)
            {
                Stop();
            }
            else
            {
                CurrentNode = runtimeEngine.Update();
            }
        }

        #region Unity Methods

        private void Start()
        {
            if (startAutomatically == false)
            {
                return;
            }

            Run();
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
