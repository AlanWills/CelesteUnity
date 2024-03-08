using Celeste.FSM;
using System;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Runtime")]
    public class NarrativeRuntime : SceneGraph<NarrativeGraph>, ILinearRuntime
    {
        #region Properties and Fields

        ILinearRuntimeRecord ILinearRuntime.Record => Record;

        public FSMNodeUnityEvent OnNodeEnter => onNodeEnter;
        public FSMNodeUnityEvent OnNodeUpdate => onNodeUpdate;
        public FSMNodeUnityEvent OnNodeExit => onNodeExit;
        public UnityEvent OnNarrativeFinished => onNarrativeFinished;

        public ChapterRecord Record { get; set; }

        [NonSerialized] private FSMNode currentNode;
        public FSMNode CurrentNode 
        {
            get { return currentNode; }
            set
            {
                if (currentNode != value)
                {
                    currentNode = value;

                    if (Record != null && currentNode != null)
                    {
                        Record.CurrentNodePath = new FSMGraphNodePath(currentNode);
                    }
                }
            }
        }

        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : graph.startNode; }
            set { startNode = value; }
        }

        [SerializeField] private FSMNodeUnityEvent onNodeEnter = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeUpdate = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeExit = new FSMNodeUnityEvent();
        [SerializeField] private UnityEvent onNarrativeFinished = new UnityEvent();
        [SerializeField] private bool startAutomatically = true;
        [SerializeField] private bool lateUpdate = false;

        private FSMRuntimeEngine runtimeEngine;

        #endregion

        #region Factory Functions

        public static NarrativeRuntime Create(
            GameObject gameObject,
            ChapterRecord chapterRecord)
        {
            gameObject.name = nameof(NarrativeRuntime);

            NarrativeGraph narrativeGraph = chapterRecord.Chapter.NarrativeGraph;
            NarrativeRuntime runtime = gameObject.AddComponent<NarrativeRuntime>();
            runtime.graph = narrativeGraph;
            runtime.Record = chapterRecord;
            runtime.StartNode = chapterRecord.CurrentNodePath.Node;

            narrativeGraph.Runtime = runtime;
            
            return runtime;
        }

        public static NarrativeRuntime Create(
            GameObject gameObject,
            NarrativeGraph narrativeGraph)
        {
            gameObject.name = nameof(NarrativeRuntime);

            NarrativeRuntime runtime = gameObject.AddComponent<NarrativeRuntime>();
            runtime.graph = narrativeGraph;

            narrativeGraph.Runtime = runtime;

            return runtime;
        }

        #endregion

        public void Run()
        {
            currentNode = null;
            enabled = true;

            if (graph != null)
            {
                graph.Runtime = this;

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
            enabled = false;
            runtimeEngine = null;
            currentNode = null;
        }

        private void UpdateFSM()
        {
            if (runtimeEngine != null && runtimeEngine.Update() == graph.finishNode)
            {
                OnNarrativeFinished.Invoke();
                Stop();
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